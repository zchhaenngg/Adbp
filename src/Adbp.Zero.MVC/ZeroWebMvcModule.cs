using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Modules;
using Adbp.Zero.MVC.Controllers;
using Adbp.Zero;
using Abp.Web.Mvc;

namespace Adbp.Zero.MVC
{
    [DependsOn(
        typeof(ZeroEntityFrameworkModule),
        typeof(ZeroApplicationModule),
        typeof(AbpWebMvcModule))]
    public class ZeroWebMvcModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new ZeroControllerConventionalRegistrar());
            IocManager.Register<StaticResourceConfig>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
