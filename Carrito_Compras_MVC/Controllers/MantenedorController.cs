using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;

namespace Carrito_Compras_MVC.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }

        public ActionResult Marca()
        {
            return View();
        }

        public ActionResult Producto()
        {
            return View();
        }
        // UTILIZO EL "#REGION" PARA AGRUPAR LINEAS DE CODIGO
        #region CATEGORIA
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> objlista = new List<Categoria>();
            objlista = new CapaN_Categoria().Listar();
            return Json(new { data = objlista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria categoria)
        {
            object resultado;
            string mensaje = string.Empty;

            if (categoria.IdCategoria == 0)
            {
                resultado = new CapaN_Categoria().Registrar(categoria, out mensaje);
            }
            else
            {
                resultado = new CapaN_Categoria().Editar(categoria, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CapaN_Categoria().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        //--------MARCA------
        #region MARCA
        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> objlista = new List<Marca>();
            objlista = new CapaN_Marca().Listar();
            return Json(new { data = objlista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca marca)
        {
            object resultado;
            string mensaje = string.Empty;

            if (marca.IdMarca == 0)
            {
                resultado = new CapaN_Marca().Registrar(marca, out mensaje);
            }
            else
            {
                resultado = new CapaN_Marca().Editar(marca, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CapaN_Marca().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //+++++PRODUCTO++++++++++++
        #region PRODUCTO
        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> objlista = new List<Producto>();
            objlista = new CapaN_Producto().Listar();
            return Json(new { data = objlista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarProducto(string producto,HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool OperacionExistosa = true;
            bool GuardarImagenExito = true;
            //Convierto el string producto en OBJETO producto↓
            Producto objProducto = new Producto();
            objProducto = JsonConvert.DeserializeObject<Producto>(producto);

            decimal precio;

            if(decimal.TryParse(objProducto.PrecioTexto,NumberStyles.AllowDecimalPoint, new CultureInfo("es-AR"),out precio))
            {
                objProducto.Precio = precio;
            }
            else
            {
                return Json(new { OperacionExistosa = false, mensaje = "El formato del precio debe ser ##.##" },JsonRequestBehavior.AllowGet);
            }



            if (objProducto.IdProducto == 0)
            {
                int IdProductoGenerado = new CapaN_Producto().Registrar(objProducto, out mensaje);

                if(IdProductoGenerado != 0)
                {
                    objProducto.IdProducto = IdProductoGenerado;
                }
                else
                {
                    OperacionExistosa = false;
                }
            }
            else
            {
                OperacionExistosa = new CapaN_Producto().Editar(objProducto, out mensaje);
            }
            if (OperacionExistosa)
            {
                if(archivoImagen != null)
                {
                    string GuardarRuta = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extencion = Path.GetExtension(archivoImagen.FileName);
                    string imagenNombre = string.Concat(objProducto.IdProducto.ToString(),extencion);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(GuardarRuta, imagenNombre));
                    }
                    catch (Exception ex)
                    {
                        string msj = ex.Message;
                        GuardarImagenExito = false;
                    }
                    if (GuardarImagenExito)
                    {
                        objProducto.RutaImagen = GuardarRuta;
                        objProducto.NombreImagen = imagenNombre;
                        bool respuesta = new CapaN_Producto().GuardarDatosImagen(objProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto pero hubo problemas con la imagen";
                    }
                }
            }


            return Json(new { OperacionExistosa = OperacionExistosa,IdGenerado = objProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto objProducto = new CapaN_Producto().Listar().Where(P => P.IdProducto == id).FirstOrDefault();
            string textoBase64 = CapaNegRecursos.ConvertirBase64(Path.Combine(objProducto.RutaImagen,objProducto.NombreImagen),out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(objProducto.NombreImagen)
            },
             JsonRequestBehavior.AllowGet
            ); 

        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CapaN_Producto().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion 
    }

}