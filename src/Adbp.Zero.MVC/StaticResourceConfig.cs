using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adbp.Web.Models;

namespace Adbp.Zero.MVC
{
    public class StaticResourceConfig
    {
        public static string AdbpJsPath = "~/wwwroot/libs/adbp";

        protected virtual IEnumerable<StaticResource> GetResources(StaticReourceNames reourceName)
        {
            switch (reourceName)
            {
                case StaticReourceNames.Basic:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/babel-polyfill/polyfill.js");
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/jquery/jquery-{version}.js");
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/jquery.form/jquery.form.js");

                    break;
                case StaticReourceNames.MomentJs:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/moment/min/moment.min.js");
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/moment/locale/zh-cn.js");
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/moment/locale/en-us.js");

                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.moment.js");
                    break;
                case StaticReourceNames.Popper:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/popper.js/umd/popper.js");
                    break;
                case StaticReourceNames.Sweetalert:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/sweetalert/sweetalert.min.js");

                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.sweetalert.js");
                    break;
                case StaticReourceNames.Toastr:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/toastr/toastr.min.js");
                    yield return new StaticResource(StaticResourceType.CSS, "~/wwwroot/libs/toastr/toastr.css");

                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.toastr.js");
                    break;
                case StaticReourceNames.SpinJs:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/spin.js/spin.js");
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/spin.js/jquery.spin.js");

                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.spin.js");
                    break;
                case StaticReourceNames.BlockUI:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/jquery.blockui/jquery.blockUI.js");

                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.blockUI.js");
                    break;
                case StaticReourceNames.Signalr:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/signalR/jquery.signalR-{version}.js");
                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.signalr.js") { Tag = "2" };
                    break;
                case StaticReourceNames.Bootstrap:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/bootstrap/js/bootstrap.js");
                    yield return new StaticResource(StaticResourceType.CSS, "~/wwwroot/libs/bootstrap/css/bootstrap.min.css", canBundle:false);
                    break;
                case StaticReourceNames.FormJs:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/daterangepicker/daterangepicker.js");
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/typeahead/typeahead.bundle.js");

                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.typeahead.js");
                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.js");
                    break;
                case StaticReourceNames.Jstree:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/jstree/jstree.js");
                    yield return new StaticResource(StaticResourceType.CSS, "~/wwwroot/libs/jstree/themes/default/style.css");

                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.jstree.js");
                    break;
                case StaticReourceNames.Datatables:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/jquery.datatables/js/jquery.dataTables.js") { Tag = "2" };
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/jquery.datatables/js/dataTables.select.js") { Tag = "2" };
                    yield return new StaticResource(StaticResourceType.CSS, "~/wwwroot/libs/jquery.datatables/css/jquery.dataTables.css");
                    yield return new StaticResource(StaticResourceType.CSS, "~/wwwroot/libs/jquery.datatables/css/select.dataTables.css");

                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.datatables.js") { Tag = "2" };
                    break;
                case StaticReourceNames.DatarangePicker:
                    yield return new StaticResource(StaticResourceType.CSS, "~/wwwroot/libs/daterangepicker/daterangepicker.css");
                    break;
                case StaticReourceNames.Fontawesome:
                    yield return new StaticResource(StaticResourceType.CSS, "~/wwwroot/libs/font-awesome/css/fontawesome-all.css");
                    break;
                case StaticReourceNames.MaterialIcons:
                    yield return new StaticResource(StaticResourceType.CSS, "~/wwwroot/libs/material-icons/material-icons.css");
                    break;
                case StaticReourceNames.Iconic:
                    yield return new StaticResource(StaticResourceType.CSS, "~/wwwroot/libs/iconic/css/open-iconic-bootstrap.css");
                    break;
                case StaticReourceNames.Abp:
                    yield return new StaticResource(StaticResourceType.JS, "~/wwwroot/libs/abp/abp.js");
                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.jquery.js");
                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.validate.js");
                    yield return new StaticResource(StaticResourceType.JS, $"{AdbpJsPath}/adbp.ajaxForm.js");
                    break;
                case StaticReourceNames.Custom://完全自定义的，覆盖插件的...  放在UI最后面
                    yield return new StaticResource(StaticResourceType.CSS, $"~/wwwroot/libs/adbp/css/adbp.checkbox.css");
                    yield return new StaticResource(StaticResourceType.CSS, $"~/wwwroot/libs/adbp/css/adbp.datatable.css");
                    yield return new StaticResource(StaticResourceType.CSS, $"~/wwwroot/libs/adbp/css/adbp.menu.css");
                    yield return new StaticResource(StaticResourceType.CSS, $"~/wwwroot/libs/adbp/css/adbp.layout.css");
                    yield return new StaticResource(StaticResourceType.CSS, $"~/wwwroot/libs/adbp/css/adbp.css");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual IEnumerable<StaticResource> GetBundleJsResources(params StaticReourceNames[] reourceNames)
        {
            foreach (var reourceName in reourceNames)
            {
                var resources = GetResources(reourceName)
                    .Where(x => x.ResourceType == StaticResourceType.JS && x.CanBundle)
                    .ToList();
                foreach (var resource in resources)
                {
                    yield return resource;
                }
            }
        }

        public virtual IEnumerable<StaticResource> GetBundleCSSResources(params StaticReourceNames[] reourceNames)
        {
            foreach (var reourceName in reourceNames)
            {
                var resources = GetResources(reourceName)
                    .Where(x => x.ResourceType == StaticResourceType.CSS && x.CanBundle)
                    .ToList();
                foreach (var resource in resources)
                {
                    yield return resource;
                }
            }
        }

        protected virtual StaticReourceNames[] BundleDependencies
            => new StaticReourceNames[] 
            {
                StaticReourceNames.Fontawesome,
                StaticReourceNames.MaterialIcons,
                StaticReourceNames.Iconic,

                StaticReourceNames.Basic,
                StaticReourceNames.Abp,
                StaticReourceNames.MomentJs,
                StaticReourceNames.Popper,
                StaticReourceNames.Sweetalert,
                StaticReourceNames.Toastr,
                StaticReourceNames.SpinJs,
                StaticReourceNames.BlockUI,
                StaticReourceNames.Signalr,
                StaticReourceNames.Bootstrap,
                StaticReourceNames.FormJs,
                StaticReourceNames.Jstree,
                StaticReourceNames.Datatables,
                StaticReourceNames.DatarangePicker,

                StaticReourceNames.Custom
            };

        /// <summary>
        /// 有些js放在一起Bundle会报错
        /// </summary>
        /// <returns></returns>
        public virtual string[] BundlesJs()
        {
            return GetBundleJsResources(BundleDependencies).Where(x=>x.Tag != "2").Select(x => x.VirtualPath).ToArray();
        }

        public virtual string[] BundlesJs2()
        {
            return GetBundleJsResources(BundleDependencies).Where(x => x.Tag == "2").Select(x => x.VirtualPath).ToArray();
        }

        public virtual string[] BundlesCss()
        {
            return GetBundleCSSResources(BundleDependencies).Select(x => x.VirtualPath).ToArray();
        }
    }

    public enum StaticReourceNames
    {
        Basic,
        MomentJs,
        Popper,
        Sweetalert,
        Toastr,
        SpinJs,
        BlockUI,
        Signalr,
        Bootstrap,
        FormJs,
        Jstree,
        Datatables,

        DatarangePicker,

        Fontawesome,
        MaterialIcons,
        Iconic,
        Abp,
        Custom
    }
}
