using System.Web;
using System.Web.Optimization;

namespace Adbp.Sample.Web
{
    public class BundleConfig
    {
#if DEBUG
        public static string AdbpJsPath = "~/AdbpJs";
#endif
#if !DEBUG
        public static string AdbpJsPath = "~/wwwroot/es5/adbp";
#endif

        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include("~/wwwroot/libs/babel-polyfill/polyfill.js")//支持es6
                .Include("~/wwwroot/libs/moment/min/moment.min.js",//momentjs
                    "~/wwwroot/libs/moment/locale/zh-cn.js",
                    "~/wwwroot/libs/moment/locale/en-us.js")
                .Include("~/wwwroot/libs/spin.js/spin.js",//spin
                    "~/wwwroot/libs/spin.js/jquery.spin.js")
                .Include("~/wwwroot/libs/popper.js/umd/popper.js")//popper
                .Include("~/wwwroot/libs/sweetalert/sweetalert.min.js")//sweetalert
                .Include("~/wwwroot/libs/jquery/jquery-{version}.js")//jquery及依赖Jquery
                .Include("~/wwwroot/libs/daterangepicker/daterangepicker.js")//依赖 jquery 以及 momentjs

                .Include("~/wwwroot/libs/typeahead/typeahead.bundle.js")//依赖Jquery 

                .Include("~/wwwroot/libs/jquery.blockui/jquery.blockUI.js")
                .Include("~/wwwroot/libs/toastr/toastr.min.js")
                .Include("~/wwwroot/libs/bootstrap/js/bootstrap.js")
                .Include("~/wwwroot/libs/jquery.form/jquery.form.js")
                .Include("~/wwwroot/libs/jstree/jstree.js")
                );

            bundles.Add(new StyleBundle("~/bundles/css")
                    .Include("~/wwwroot/libs/font-awesome/css/fontawesome-all.css", new CssRewriteUrlTransform())
                    .Include("~/wwwroot/libs/material-icons/material-icons.css", new CssRewriteUrlTransform())
                    .Include("~/wwwroot/libs/iconic/css/open-iconic-bootstrap.css", new CssRewriteUrlTransform())
                    .Include("~/wwwroot/libs/toastr/toastr.css", new CssRewriteUrlTransform())
                    //.Include("~/wwwroot/libs/bootstrap/css/bootstrap.css", new CssRewriteUrlTransform()) //无法压缩
                    .Include("~/wwwroot/libs/jstree/themes/default/style.css", new CssRewriteUrlTransform())
                    .Include("~/wwwroot/libs/daterangepicker/daterangepicker.css", new CssRewriteUrlTransform())
                );

#region jquery.datatables.js
            bundles.Add(new ScriptBundle("~/bundles/datatablesjs")
                .Include("~/wwwroot/libs/jquery.datatables/js/jquery.dataTables.js")
                .Include("~/wwwroot/libs/jquery.datatables/js/dataTables.select.js"));

            bundles.Add(new StyleBundle("~/bundles/datatablesCss")
                .Include("~/wwwroot/libs/jquery.datatables/css/jquery.dataTables.css", new CssRewriteUrlTransform())
                .Include("~/wwwroot/libs/jquery.datatables/css/select.dataTables.css", new CssRewriteUrlTransform())
                );
#endregion
            
#region adbp
            bundles.Add(new ScriptBundle("~/bundles/adbpjs")
                .Include("~/wwwroot/libs/abp/abp.js")//abp
                .Include($"{AdbpJsPath}/adbp.sweetalert.js")
                .Include($"{AdbpJsPath}/adbp.toastr.js")
                .Include($"{AdbpJsPath}/adbp.spin.js")
                .Include($"{AdbpJsPath}/adbp.moment.js")
                .Include($"{AdbpJsPath}/adbp.jquery.js")
                .Include($"{AdbpJsPath}/adbp.validate.js")
                .Include($"{AdbpJsPath}/adbp.ajaxForm.js")
                .Include($"{AdbpJsPath}/adbp.datatables.js")
                .Include($"{AdbpJsPath}/adbp.jstree.js")
                .Include($"{AdbpJsPath}/adbp.blockUI.js")

                .Include($"{AdbpJsPath}/adbp.typeahead.js")
                .Include($"{AdbpJsPath}/adbp.js")
                       );

            bundles.Add(new StyleBundle("~/bundles/adbpCss")
                .Include($"~/wwwroot/libs/adbp/css/adbp.checkbox.css", new CssRewriteUrlTransform())
                .Include($"~/wwwroot/libs/adbp/css/adbp.datatable.css", new CssRewriteUrlTransform())
                .Include($"~/wwwroot/libs/adbp/css/adbp.menu.css", new CssRewriteUrlTransform())
                .Include($"~/wwwroot/libs/adbp/css/adbp.layout.css", new CssRewriteUrlTransform())
                .Include($"~/wwwroot/libs/adbp/css/adbp.css", new CssRewriteUrlTransform())
                );
#endregion
        }
    }
}
