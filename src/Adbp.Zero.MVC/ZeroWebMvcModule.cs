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

namespace Adbp.Zero.MVC
{
    [DependsOn(
        typeof(ZeroApplicationModule))]
    public class ZeroWebMvcModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new ZeroControllerConventionalRegistrar());
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
