using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DetalleProducto(int idProducto = 0)
        {
            Producto producto = new Producto();
            bool conversion;

            producto = new CapaN_Producto().Listar().Where(p => p.IdProducto == idProducto).FirstOrDefault();
            if(producto != null)
            {     //Utilizo Path para combinar las rutas
                producto.Base64 = CapaNegRecursos.ConvertirBase64(Path.Combine(producto.RutaImagen, producto.NombreImagen), out conversion);
                producto.Extension = Path.GetExtension(producto.NombreImagen);
            }


            return View(producto);
        }

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            lista = new CapaN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarMarcaPorCategoria(int idCategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CapaN_Marca().ListarMarcaPorCategoria(idCategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarProducto(int idCategoria,int idMarca)
        {
            List<Producto> lista = new List<Producto>();
            bool conversion;

            lista = new CapaN_Producto().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                ObjMarca = p.ObjMarca,
                ObjCategoria = p.ObjCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CapaNegRecursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>
            p.ObjCategoria.IdCategoria == (idCategoria == 0 ? p.ObjCategoria.IdCategoria : idCategoria) &&
            p.ObjMarca.IdMarca == (idMarca == 0 ? p.ObjMarca.IdMarca : idMarca) &&
            p.Stock > 0 && p.Activo == true
            ).ToList();

            var jsonResult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;

        }
        [HttpPost]

        public JsonResult AgregarAlCarrito(int idproducto)
        {
            //Yo solo quiero el ID del cliente en session,entonces convierto la Session en un objeto Cliente
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool existe = new CapaN_Carrito().ExisteCarrito(idcliente, idproducto);

            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";    
            }
            else
            {
                respuesta = new CapaN_Carrito().OperacionCarrito(idcliente, idproducto,true, out mensaje);
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CapaN_Carrito().CantidadEnCarrito(idcliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            List<Carrito> lista = new List<Carrito>();
            bool conversion;

            lista = new CapaN_Carrito().ListarProductos(idcliente).Select(oc => new Carrito()
            {
                ObjProducto = new Producto()
                {
                    IdProducto = oc.ObjProducto.IdProducto,
                    Nombre = oc.ObjProducto.Nombre,
                    ObjMarca = oc.ObjProducto.ObjMarca,
                    Precio = oc.ObjProducto.Precio,
                    RutaImagen = oc.ObjProducto.RutaImagen,
                    Base64 = CapaNegRecursos.ConvertirBase64(Path.Combine(oc.ObjProducto.RutaImagen, oc.ObjProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.ObjProducto.NombreImagen)


                },
                Cantidad = oc.Cantidad
            }).ToList();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto,bool sumar)
        {
          
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CapaN_Carrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CapaN_Carrito().EliminarCarrito(idcliente, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ObtenerProvincia()
        {
            List<Provincia> oLista = new List<Provincia>();

            oLista = new CapaN_Ubicacion().ObtenerProvincia();

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ObtenerCiudad(string IdProvincia)
        {
            List<Ciudad> oLista = new List<Ciudad>();

            oLista = new CapaN_Ubicacion().ObtenerCiudad(IdProvincia);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost]
        //Creo un metodo asíncrono por los servicios de PayPal y creo una tarea (Task)
        public async Task<JsonResult> ProcesarPago(List<Carrito> listaCarrito, Venta oVenta)
        {
            decimal total = 0;
            DataTable detalle_venta = new DataTable();

            detalle_venta.Locale = new CultureInfo("es-AR");
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            foreach(Carrito ObjCarrito in listaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(ObjCarrito.Cantidad.ToString()) * ObjCarrito.ObjProducto.Precio;

                total += subtotal;

                detalle_venta.Rows.Add(new object[]
                {
                    ObjCarrito.ObjProducto.IdProducto,
                    ObjCarrito.Cantidad,
                    subtotal
                });
            }
            oVenta.MontoTotal = total;
            oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            return Json(new { Status = true, Link = "/Tienda/PagoEfectuado?idTransaccion=code=0001&status=true" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> PagoEfectuado()
        {
            string idtransaccion = Request.QueryString["idTransaccion"];
            bool status = Convert.ToBoolean( Request.QueryString["status"]);

            ViewData["Status"] = status;

            if (status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                oVenta.IdTransaccion = idtransaccion;
                string Mensaje = string.Empty;

                bool respuesta = new CapaN_Venta().Registrar(oVenta, detalle_venta, out Mensaje);
                //Este viewData me va a permitir compartir el Id de la transaccion con la vista.
                ViewData["idTransaccion"] = oVenta.IdTransaccion;
            }
            return View();
        }
    }
}