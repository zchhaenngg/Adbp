using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;
using System.Linq;
using Adbp.Zero.MVC;
using System;
using Adbp.Web.Models;

namespace CompanyName.ProjectName.Web
{
    public class BundleConfig
    {
        private readonly StaticResourceConfig _config;
        
        public BundleConfig(StaticResourceConfig config)
        {
            _config = config;
        }
        
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new StyleBundle("~/bundles/css").IncludeCss(
                _config.BundlesCss()
                ));

            bundles.Add(new ScriptBundle("~/bundles/js").IncludeJs(
               _config.BundlesJs()
               ));
            
            bundles.Add(new ScriptBundle("~/bundles/js2").IncludeJs(
               _config.BundlesJs2()
               ));

        }
    }
}
