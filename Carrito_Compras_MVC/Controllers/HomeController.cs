using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;

namespace Carrito_Compras_MVC.Controllers
{
    //Para ingresar a estos formularios debes estar logeado(Administrador)
    [Authorize]
    
    public class HomeController : Controller
    {
      
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuario()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> objlista = new List<Usuario>();
            objlista = new CapaN_Usuario().Listar();

            return Json(new { data = objlista },JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GuardarUsuario(Usuario user)
        {
            object resultado;
            string mensaje = string.Empty;

            if(user.IdUsuario == 0)
            {
                resultado = new CapaN_Usuario().Registrar(user, out mensaje);
            }
            else
            {
                resultado = new CapaN_Usuario().Editar(user, out mensaje);
            }
            return Json(new { resultado = resultado,mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CapaN_Usuario().Eliminar(id,out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaReporte(string fechaInicio,string fechaFin,string idTransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            lista = new CapaN_Reporte().Ventas(fechaInicio,fechaFin,idTransaccion);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VistaCuadroDeMando()
        {
            CuadroDeMando objeto = new CapaN_Reporte().VerCuadroDeMando();

            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVenta(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> lista = new List<Reporte>();
            lista = new CapaN_Reporte().Ventas(fechaInicio, fechaFin, idTransaccion);

            DataTable dataTable = new DataTable();

            dataTable.Locale = new CultureInfo("es-AR");
            dataTable.Columns.Add("Fecha Venta", typeof(string));
            dataTable.Columns.Add("Cliente", typeof(string));
            dataTable.Columns.Add("Producto", typeof(string));
            dataTable.Columns.Add("Precio", typeof(decimal));
            dataTable.Columns.Add("Cantidad", typeof(int));
            dataTable.Columns.Add("Total", typeof(decimal));
            dataTable.Columns.Add("IdTransaccion", typeof(string));

            foreach(Reporte rp in lista)
            {
                dataTable.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }
            dataTable.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Con esto le digo que el File(Archivo) que va a descargar es un Excel⬇️⬇️
                    return File(stream.ToArray(),"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                }


            }
        }
    }
}