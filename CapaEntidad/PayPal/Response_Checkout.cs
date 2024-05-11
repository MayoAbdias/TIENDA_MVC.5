using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.PayPal
{
    public class Response_Checkout
    {
        public string id { get; set; }
        public string status { get; set; }
        public PaymentSource payment_source { get; set; }
        public List<Link> links { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }

    //public class PaymentSource
    //{
    //    public Paypal paypal { get; set; }
    //}

    //public class Paypal
    //{
    //}
}
