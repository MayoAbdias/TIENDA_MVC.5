using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(Cliente cliente)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(cliente.Nombres) ? "" : cliente.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(cliente.Apellidos) ? "" : cliente.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(cliente.Correo) ? "" : cliente.Correo;

            if(cliente.Clave != cliente.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new CapaN_Cliente().Registrar(cliente, out mensaje);

            if(resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Login(string correo,string clave)
        {
            Cliente cliente = null;
            cliente = new CapaN_Cliente().Listar().Where(c => c.Correo == correo && c.Clave == CapaNegRecursos.ConvertirSha256(clave)).FirstOrDefault();

            if(cliente == null)
            {
                ViewBag.Error = "El correo o la contraseña no son correctas";
                return View();
            }
            else
            {
                if (cliente.Reestablecer)
                {
                    TempData["idCliente"] = cliente.IdCliente;
                    return RedirectToAction("CambiarClave","Acceso");
                }
                else
                {
                    //Creo una autenticacion por Cookie del usuario(cliente) atravez de su correo//El valor de esa Cookie la 
                    //vamos a obtener a travez de User.Identity.Name que esta en el Layout (El name que guardamos es el correo)
                    FormsAuthentication.SetAuthCookie(cliente.Correo, false);
                    //En session guardo toda la info del cliente
                    Session["Cliente"] = cliente;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }

            
        }
        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente cliente = new Cliente();

            cliente = new CapaN_Cliente().Listar().Where(u => u.Correo == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();
            }
            string mensaje = string.Empty;
            bool respuesta = new CapaN_Cliente().ReestablecerClave(cliente.IdCliente, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult CambiarClave(string idCliente,string claveActual,string nuevaClave,string confirmarClave)
        {
            Cliente cliente = new Cliente();
            cliente = new CapaN_Cliente().Listar().Where(c => c.IdCliente == int.Parse(idCliente)).FirstOrDefault();

            if (cliente.Clave != CapaNegRecursos.ConvertirSha256(claveActual))
            {
                TempData["IdCliente"] = idCliente;
                //viewData lo utilizo para almacenar valores mas simples como cadenas de texto
                ViewData["valorClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaClave != confirmarClave)
            {
                TempData["IdCliente"] = idCliente;
                ViewData["valorClave"] = claveActual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["valorClave"] = "";

            nuevaClave = CapaNegRecursos.ConvertirSha256(nuevaClave);
            string mensaje = string.Empty;

            bool respuesta = new CapaN_Cliente().CambiarClave(int.Parse(idCliente), nuevaClave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Login");
            }
            else
            {
                TempData["IdCliente"] = idCliente;
                ViewBag.Error = mensaje;
                return View();

            }
           
        }
        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Acceso");
        }
    }
}