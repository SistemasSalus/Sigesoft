using System.Web;
using System.Web.Optimization;

namespace MvcSigesoft
{
    public class BundleConfig
    {
        // Para obtener más información acerca de Bundling, consulte http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                          "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Scripts/nifty.js",
                      //"~/Scripts/knockout-3.3.0.js",
                      //"~/Scripts/knockout-3.3.0.debug.js",
                      //"~/Scripts/knockout.mapping-latest.js",
                      //"~/Scripts/knockout.validation.js",
                      "~/plugins/bootstrap-select/bootstrap-select.min.js",
                      //"~/plugins/switchery/switchery.min.js",
                      "~/plugins/bootstrap-datepicker/bootstrap-datepicker.min.js"
                      //"~/plugins/jstree/jstree.min.js",
                      //"~/plugins/bootstrap-wizard/jquery.bootstrap.wizard.min.js",
                      //"~/plugins/bootstrap-validator/bootstrapValidator.min.js",
                      //"~/Scripts/demo/form-wizard.js",
                      //"~/plugins/chosen/chosen.jquery.min.js"
                      //"~/plugins/fooTable/dist/footable.all.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/dataTable").Include(
                     "~/plugins/datatables/media/js/jquery.dataTables.js",
                     "~/plugins/datatables/media/js/dataTables.bootstrap.js",
                     "~/plugins/datatables/extensions/Responsive/js/dataTables.responsive.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/flotcharts").Include(
                     "~/plugins/flot-charts/jquery.flot.min.js",
                     "~/plugins/flot-charts/jquery.flot.resize.min.js",
                     "~/plugins/flot-charts/jquery.flot.pie.min.js",
                     "~/plugins/flot-charts/jquery.flot.tooltip.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/icons").Include(
                     "~/Scripts/demo/icons.js"
                     ));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/nifty.css",
                      "~/Content/demo/nifty-demo-icons.css",
                      "~/Content/demo/nifty-demo.min.css",
                      "~/plugins/bootstrap-select/bootstrap-select.min.css",                      
                      //"~/plugins/switchery/switchery.min.css",
                      "~/plugins/bootstrap-datepicker/bootstrap-datepicker.min.css"
                      //"~/plugins/jstree/themes/default/style.min.css",
                      //"~/plugins/fullcalendar/fullcalendar.min.css",
                      //"~/plugins/fullcalendar/nifty-skin/fullcalendar-nifty.min.css",
                      //"~/plugins/chosen/chosen.min.css"
                      //"~/plugins/fooTable/css/footable.core.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/dataTable").Include(
                    "~/plugins/datatables/media/css/dataTables.bootstrap.css",
                    "~/plugins/datatables/extensions/Responsive/css/responsive.dataTables.css"
                    ));

            bundles.Add(new StyleBundle("~/Content/icons").Include(
                    "~/plugins/ionicons/css/ionicons.css",
                    "~/plugins/font-awesome/css/font-awesome.css"
                    ));
        }
    }
}