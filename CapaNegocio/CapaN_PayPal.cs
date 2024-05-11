using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CapaEntidad.PayPal;

namespace CapaNegocio
{
    public class CapaN_PayPal
    {
        //Los valores de url,client y secret son de Paypal y los trigo del WebConfig(que es donde estan guardados sus valores)
        private static string urlpaypal = ConfigurationManager.AppSettings["UrlPaypal"];
        private static string clientid = ConfigurationManager.AppSettings["ClientId"];
        private static string secret = ConfigurationManager.AppSettings["Secret"];

        public async Task<Response_Paypal<Response_Checkout>> CrearSolicitud (Checkout_order orden)
        {
            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlpaypal);

                var authToken = Encoding.ASCII.GetBytes($"{clientid}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",Convert.ToBase64String(authToken));

                //Convierto la clase en un objeto de Json
                var json = JsonConvert.SerializeObject(orden);
                //Ahora lo convierto en el tipo de contenido que necesita la API
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                //Momento de ejecutar la API
                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);//obtengo la respuesta.

                response_paypal.Status = response.IsSuccessStatusCode;//Esto me dice si la solicitud tuvo exito o no.

                if (response.IsSuccessStatusCode)
                {
                    string jsonRespuesta = response.Content.ReadAsStringAsync().Result;//Con esto puedo leer la respuesta que esta dentro de un string.

                    //Ahora convierto ESE string en una clase.
                    Response_Checkout checkout = JsonConvert.DeserializeObject<Response_Checkout>(jsonRespuesta);

                    response_paypal.Response = checkout;
                }
                return response_paypal;
            }
        }
        public async Task<Response_Paypal<Response_Capture>> AprobarPago(string token)
        {
            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlpaypal);

                var authToken = Encoding.ASCII.GetBytes($"{clientid}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                //Envio un json vacio
                var data = new StringContent("{}", Encoding.UTF8, "application/json");
                //Momento de ejecutar la API                          le paso el parametro token              
                HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);//obtengo la respuesta.

                response_paypal.Status = response.IsSuccessStatusCode;//Esto me dice si la solicitud tuvo exito o no.

                if (response.IsSuccessStatusCode)
                {
                    string jsonRespuesta = response.Content.ReadAsStringAsync().Result;//Con esto puedo leer la respuesta que esta dentro de un string.

                    //Ahora convierto ESE string en una clase.
                    Response_Capture capture = JsonConvert.DeserializeObject<Response_Capture>(jsonRespuesta);

                    response_paypal.Response = capture;
                }
                return response_paypal;
            }
        }
    }
}
