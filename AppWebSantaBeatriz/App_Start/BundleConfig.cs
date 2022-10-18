using System.Web;
using System.Web.Optimization;

namespace AppWebSantaBeatriz
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            //Jquery
            //===============================================================================================================
            

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.6.0.min.js"));
            //===============================================================================================================



            //var jqueryValCdnPath = "https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.2/jquery.validate.min.js";
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate.min.js", "~/Scripts/additional-methods.min.js"));

            ////Modernizr
            ////===============================================================================================================

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-2.8.3.js"));
        ////===============================================================================================================
       
            ////Popper
            ////===============================================================================================================

            bundles.Add(new ScriptBundle("~/bundles/popper").Include("~/Scripts/popper.js"));
            ////===============================================================================================================

            ////Bootstrap
            ////===============================================================================================================
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.bundle.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.min.css"));
            ////===============================================================================================================

            ////JqueryUi
            ////===============================================================================================================
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
             "~/Scripts/jquery-ui-1.12.1.min.js"));
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
              "~/Content/themes/base/jquery-ui.min.css"
              ));

            ////===============================================================================================================
        }
    }
}
