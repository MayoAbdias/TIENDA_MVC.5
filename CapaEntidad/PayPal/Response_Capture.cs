using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.PayPal
{
    public class Response_Capture
    {
        public string id { get; set; }
        public string status { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public List<Link> links { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class Amount
    //{
    //    public string currency_code { get; set; }
    //    public string value { get; set; }
    //}

    public class Capture
    {
        public string id { get; set; }
        public string status { get; set; }
        public Amount amount { get; set; }
        public SellerProtection seller_protection { get; set; }
        public bool final_capture { get; set; }
        public SellerReceivableBreakdown seller_receivable_breakdown { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public List<Link> links { get; set; }
    }

    public class GrossAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string sku { get; set; }
        public string quantity { get; set; }
    }

    //public class Link
    //{
    //    public string href { get; set; }
    //    public string rel { get; set; }
    //    public string method { get; set; }
    //}

    public class NetAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Payments
    {
        public List<Capture> captures { get; set; }
    }

    public class PaypalFee
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    //public class PurchaseUnit
    //{
    //    public string reference_id { get; set; }
    //    public List<Item> items { get; set; }
    //    public Shipping shipping { get; set; }
    //    public Payments payments { get; set; }
    //}
    public class SellerProtection
    {
        public string status { get; set; }
    }

    public class SellerReceivableBreakdown
    {
        public GrossAmount gross_amount { get; set; }
        public PaypalFee paypal_fee { get; set; }
        public NetAmount net_amount { get; set; }
    }

    public class Shipping
    {
        public List<Tracker> trackers { get; set; }
    }

    public class Tracker
    {
        public string id { get; set; }
        public List<Link> links { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
    }


}
