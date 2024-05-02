using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace Carrito_Compras_MVC.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string correo,string clave)
        {
            Usuario usuario = new Usuario();
              //Utilizo el where para filtrar y ver si encontro un usuario con las credenciales que esta recibiendo.
            usuario = new CapaN_Usuario().Listar().Where(u => u.Correo == correo && u.Clave == CapaNegRecursos.ConvertirSha256(clave)).FirstOrDefault();

            //Valido si es que encontro un usuario con las credeciales ingresadas.
            if(usuario == null)
            { 
                //El ViexBag. nos sirve para guardar informacion(es este caso un mensaje de error.) y compartirla con la vista que en este caso es Index.
                ViewBag.Error = "Correo o contraseña incorrecta";
                return View();
            }
            else
            {
                if (usuario.Reestablecer)
                {
                    //El TempData tambien nos sirve para guardar informacion pero esta info se puede compartir con VARIAS vistas que
                    //esten dentro del mismo controlador.
                    TempData["IdUsuario"] = usuario.IdUsuario;
         //En este caso para redireccionar a otra vista no escribo su controlador ya que a la vista que la redirijo
         //esta dentro del mismo controlador (Acceso) Entonces no hace falta escribir el controlador al lado⬇️
                    return RedirectToAction("CambiarClave");
                }
                //FormsAuthentication para que no puedas acceder a los formularios si no estas logeado(Admin)
                // Agregando la referencia de : using System.Web.Security 
                FormsAuthentication.SetAuthCookie(usuario.Correo, false);
                    

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }          
        }
        [HttpPost]
        public ActionResult CambiarClave(string idUsuario,string claveActual,string nuevaClave,string confirmarClave)
        {
            Usuario usuario = new Usuario();
            usuario = new CapaN_Usuario().Listar().Where(u => u.IdUsuario == int.Parse(idUsuario)).FirstOrDefault();

            if(usuario.Clave != CapaNegRecursos.ConvertirSha256(claveActual))
            {
                TempData["IdUsuario"] = idUsuario;
                //viewData lo utilizo para almacenar valores mas simples como cadenas de texto
                ViewData["valorClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if(nuevaClave != confirmarClave)
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["valorClave"] = claveActual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["valorClave"] = "";

            nuevaClave = CapaNegRecursos.ConvertirSha256(nuevaClave);
            string mensaje = string.Empty;

            bool respuesta = new CapaN_Usuario().CambiarClave(int.Parse(idUsuario), nuevaClave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = idUsuario;
                ViewBag.Error = mensaje;
                return View();

            }
        }
        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario usuario = new Usuario();

            usuario = new CapaN_Usuario().Listar().Where(u => u.Correo == correo).FirstOrDefault();

            if(usuario == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();               
            }
            string mensaje = string.Empty;
            bool respuesta = new CapaN_Usuario().ReestablecerClave(usuario.IdUsuario, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}