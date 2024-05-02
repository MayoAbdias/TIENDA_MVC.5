using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
//Agrego referencias para poder trabajar la parte de Email.
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Security.Policy;

namespace CapaNegocio
{

   
    public class CapaNegRecursos
    {

        public static string GenerarClave()
        { //el Guid permite retortan una clave unica.Formato de "N"(caracteres alfanumericos).Substring cantidad de caracteres(8)
            string clave = Guid.NewGuid().ToString("N").Substring(0, 8);
            return clave;
        }


        //Encriptacion de texto(Clave) en SHA256
        public static string ConvertirSha256(string texto)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //Usar la referencia de "System.Security.Cryptography"
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding encoding = Encoding.UTF8;
                byte[] result = hash.ComputeHash(encoding.GetBytes(texto));

                foreach (byte b in result)
                    stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        public static bool EnviarCorreo(string correo,string asunto,string mensaje)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("mayoezequiel99@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("mayoezequiel99@gmail.com", "swqe ipwb ysmh gtth"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true

                };
                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception ex)
            {

                resultado = false;
            }
            return resultado;
        }
        public static string ConvertirBase64(string ruta,out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                conversion = false;               
            }
            return textoBase64;
        }
    }
}
