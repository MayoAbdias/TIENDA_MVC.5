using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaN_Cliente
    {
        private CapaD_Cliente objCapaDato = new CapaD_Cliente();

        public int Registrar(Cliente user, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(user.Nombres) || string.IsNullOrWhiteSpace(user.Nombres))
            {
                Mensaje = "El Nombre del Cliente no puede estar vacio ";

            }
            else if (string.IsNullOrEmpty(user.Apellidos) || string.IsNullOrWhiteSpace(user.Apellidos))
            {
                Mensaje = "El Apellido del Cliente no puede estar vacio";

            }
            else if (string.IsNullOrEmpty(user.Correo) || string.IsNullOrWhiteSpace(user.Correo))
            {
                Mensaje = "El Correo del Cliente no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                user.Clave = CapaNegRecursos.ConvertirSha256(user.Clave);
                return objCapaDato.Registrar(user, out Mensaje);
            }
            else
            {
                return 0;
            }
        }
        public List<Cliente> Listar()
        {
            return objCapaDato.Listar();
        }
        public bool CambiarClave(int idCliente, string nuevaClave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idCliente, nuevaClave, out Mensaje);
        }
        public bool ReestablecerClave(int idCliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = CapaNegRecursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idCliente, CapaNegRecursos.ConvertirSha256(nuevaClave), out Mensaje);

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
