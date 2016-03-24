using System;
using System.Web;
using System.Web.Optimization;

namespace Emaily.Web
{
    public class BundleConfig
    {
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
            {
                throw new ArgumentNullException("ignoreList");
            }

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");


            //ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
            bundles.UseCdn = true;

            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new StyleBundle("~/Content/login").Include(
                    "~/Content/stylesheets/bootstrap.css",
                    "~/Content/css-pre/simple-line-icons.css",
                    "~/Content/stylesheets/login.css",
                    "~/Content/stylesheets/darkblue.css",
                    "~/Content/stylesheets/components-rounded.css",
                    "~/Content/stylesheets/site.css"));

            bundles.Add(new ScriptBundle("~/script/vendors")
              .Include("~/App/vendors/jquery-{version}.js")
              .Include("~/App/vendors/jquery.migrate.min.js")
              .Include("~/App/vendors/bootstrap.js")
              .Include("~/App/vendors/angular.js")
              .Include("~/App/vendors/moment.js")
              .Include("~/App/vendors/bootstrap-hover-dropdown.js")
              .Include("~/App/vendors/jquery.slimscroll.js")
              .Include("~/App/vendors/jquery.blockui.min.js")
              .Include("~/App/vendors/jquery.cokie.min.js")
              .Include("~/App/vendors/bootstrap-switch.js")
              .Include("~/App/vendors/ocLazyLoad.min.js")
              .Include("~/App/vendors/ui-bootstrap-tpls.js")
              .Include("~/App/vendors/bootstrap-confirmation.js")
              .Include("~/App/vendors/bootstrap-select.js")
              .Include("~/App/vendors/select2.js")
              .Include("~/App/vendors/jquery.multi-select.js")
              .Include("~/App/vendors/rzslider.js")
              .Include("~/App/vendors/metronic.js")
              .Include("~/App/vendors/layout.js")
              .Include("~/App/vendors/extra/*.js")
              .Include("~/App/vendors/angular-*")
           );

            bundles.Add(new ScriptBundle("~/script/spa")
                .Include("~/App/app.js")
                .Include("~/App/uploader/jquery.ui.widget.js")
                .Include("~/App/uploader/load-image.js")
                .Include("~/App/uploader/load-image-ios.js")
                .Include("~/App/uploader/load-image-orientation.js")
                .Include("~/App/uploader/load-image-meta.js")
                .Include("~/App/uploader/load-image-exif.js")
                .Include("~/App/uploader/load-image-exif-map.js")
                .Include("~/App/uploader/canvas-to-blob.js")
                .Include("~/App/uploader/jquery.iframe-transport.js")
                .Include("~/App/uploader/jquery.fileupload.js")
                .Include("~/App/uploader/jquery.fileupload-process.js")
                .Include("~/App/uploader/jquery.fileupload-image.js")
                .Include("~/App/uploader/jquery.fileupload-audio.js")
                .Include("~/App/uploader/jquery.fileupload-video.js")
                .Include("~/App/uploader/jquery.fileupload-validate.js")
                .Include("~/App/uploader/jquery.fileupload-angular.js")
                .Include("~/App/modules/*.js")
                .Include("~/App/app.*")
                .Include("~/App/router/*.js")
                .Include("~/App/directives/*.js")
                .Include("~/App/services/*.js")
                .IncludeDirectory("~/App/views/", "*.js", true)
            );

            bundles.Add(new StyleBundle("~/content/spa/codemirror").Include(
               "~/scripts/code-mirror/lib/codemirror.css",
               "~/scripts/code-mirror/theme/monokai.css",
               "~/scripts/code-mirror/addon/dialog/dialog.css",
               "~/scripts/code-mirror/addon/display/fullscreen.css",
               "~/scripts/code-mirror/addon/fold/foldgutter.css",
               "~/scripts/code-mirror/addon/hint/show-hint.css",
               "~/scripts/code-mirror/addon/lint/lint.css",
               "~/scripts/code-mirror/addon/merge/merge.css"));

            bundles.Add(new ScriptBundle("~/script/spa/codemirror").Include(
                "~/scripts/code-mirror/lib/codemirror.js",
                "~/scripts/code-mirror/mode/css/css.js",
                "~/scripts/code-mirror/mode/xml/xml.js",
                "~/scripts/code-mirror/mode/javascript/javascript.js",
                "~/scripts/code-mirror/mode/htmlmixed/htmlmixed.js")
                .IncludeDirectory("~/scripts/code-mirror/addon/", "*.js", true)
            );

            bundles.Add(new StyleBundle("~/Content/spa/css")
                .Include("~/Content/css-pre/*.css")
                .Include(
                    "~/Content/css/pages/*.css",
                    "~/Content/css/custom/*.css",
                    "~/Content/css/components.css",
                    "~/Content/css/plugins.css",
                    "~/Content/css/layout.css",
                    "~/Content/css/themes/darkblue.css",
                    "~/Content/css/xtra.xs.css",
                    "~/Content/css/xtra.sm.css",
                    "~/Content/css/xtra.md.css",
                    "~/Content/css/xtra.lg.css",
                    "~/Content/css/xtra.xl.css",
                    "~/Content/css/xtra.css"
                )
            );

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));



        }
    }
}
