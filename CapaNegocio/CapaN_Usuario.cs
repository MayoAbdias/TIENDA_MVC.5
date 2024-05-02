using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Security.Cryptography;
using System.Security.Claims;

namespace CapaNegocio
{
    public class CapaN_Usuario
    {
        private CapaD_Usuarios objCapaDato = new CapaD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Usuario user, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(string.IsNullOrEmpty(user.Nombres) || string.IsNullOrWhiteSpace(user.Nombres))
            {
                Mensaje = "El Nombre del Usuario no puede estar vacio ";

            }else if(string.IsNullOrEmpty(user.Apellidos) || string.IsNullOrWhiteSpace(user.Apellidos))
            {
                Mensaje = "El Apellido del Usuario no puede estar vacio";

            }else if(string.IsNullOrEmpty(user.Correo) || string.IsNullOrWhiteSpace(user.Correo))
            {
                Mensaje = "El Correo del Usuario no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CapaNegRecursos.GenerarClave();

                string asunto = "Creacion de cuenta";

                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: ¡clave! </p>";
                mensaje_correo = mensaje_correo.Replace("¡clave!", clave);

                bool respuesta = CapaNegRecursos.EnviarCorreo(user.Correo, asunto, mensaje_correo);
                //Si la respuesta es correcta(si se envio el correo) encripto la clave
                if (respuesta)
                {
                    user.Clave = CapaNegRecursos.ConvertirSha256(clave);
                    return objCapaDato.Registrar(user, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo";
                    return 0;
                }             
            }
            else
            {
                return 0;
            }      
        }

        public bool Editar(Usuario user, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(string.IsNullOrEmpty(user.Nombres) || string.IsNullOrWhiteSpace(user.Nombres))
            {
                Mensaje = "El Nombre del Usuario no puede estar vacio";

            }else if(string.IsNullOrEmpty(user.Apellidos) || string.IsNullOrWhiteSpace(user.Apellidos))
            {
                Mensaje = "El Apellido del Usuario no puede estar vacio";

            }else if(string.IsNullOrEmpty(user.Correo) || string.IsNullOrWhiteSpace(user.Correo))
            {
                Mensaje = "El Correo del Usuario no puede estar vacio;";
            }
            if(string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(user, out Mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }
        public bool CambiarClave(int idUsuario, string nuevaClave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idUsuario, nuevaClave, out Mensaje);
        }

        public bool ReestablecerClave(int idUsuario,string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = CapaNegRecursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idUsuario,CapaNegRecursos.ConvertirSha256(nuevaClave), out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";
                string mensaje_correo = "<h3>Su contraseña fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: ¡clave! </p>";
                mensaje_correo = mensaje_correo.Replace("¡clave!", nuevaClave);

                bool respuesta = CapaNegRecursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }
        }
    }

   
}
