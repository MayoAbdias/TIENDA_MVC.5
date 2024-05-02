using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                respuesta = new CapaN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
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
        public JsonResult OperacionCarrito(int idproducto,bool sumar)
        {
          
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CapaN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);

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
    }
}