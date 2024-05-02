using System.Web;
using System.Web.Optimization;

namespace Carrito_Compras_MVC
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));


            //Llamo al Script fontaWesome que instale al igual que el DataTables de jQuery que se encuentran en la
            //carpeta Scripts
            bundles.Add(new Bundle("~/bundles/complementos").Include(
                       "~/Scripts/fontawesome/all.min.js",
                       "~/Scripts/DataTables/jquery.dataTables.js",
                     //Agrego el responsive para visualizar nuestra tabla en un entorno movil.
                       "~/Scripts/DataTables/dataTables.responsive.js",
                       "~/Scripts/LoadingOverlay/loadingoverlay.min.js",
                       "~/Scripts/sweetalert.min.js",
                       "~/Scripts/jquery.validate.js",
                       "~/Scripts/jquery-ui-1.13.2.js",
                       "~/Scripts/scripts.js"));

            // bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //  "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios.  De esta manera estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            // bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //           "~/Scripts/modernizr-*")); 

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
      //Aca  hago utilidad de del Css del DataTable que esta en la carpeta Content/DataTable/css
                      "~/Content/DataTables/css/jquery.dataTables.css",
                      "~/Content/DataTables/css/responsive.dataTables.css",
                      "~/Content/sweetalert.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/site.css"));
        }
    }
}
