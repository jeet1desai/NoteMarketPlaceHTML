using System.Web;
using System.Web.Optimization;

namespace NoteMarketPlace
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Client/jquery.js",
                        "~/Scripts/Admin/jquery.js",
                        "~/Scripts/Client/script.js",
                        "~/Scripts/Admin/script.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Client/bootstrap.min.js",
                      "~/Scripts/Client/bootstrap.bundle.min.js",
                      "~/Scripts/Admin/bootstrap.min.js",
                      "~/Scripts/Admin/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Client/bootstrap/bootstrap.min.css",
                      "~/Content/Client/style.css",
                      "~/Content/Client/responsive.css",
                      "~/Content/Admin/bootstrap/bootstrap.min.css",
                      "~/Content/Admin/style.css",
                      "~/Content/Admin/responsive.css"));
        }
    }
}
