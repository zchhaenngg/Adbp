using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Modules;
using Abp.Web.Mvc;
using Microsoft.Owin.Security;
using Castle.MicroKernel.Registration;
using Adbp.Sample.EntityFramework;
using Adbp.Sample.Api;
using Abp.Zero.Configuration;
using Abp.Timing;
using Adbp.Zero.MVC.App_Start;
using Adbp.Zero.MVC;

namespace Adbp.Sample.Web.App_Start
{
    [DependsOn(
        typeof(SampleDataModule),
        typeof(SampleApplicationModule),
        typeof(SampleWebApiModule),
        //typeof(AbpHangfireModule), - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        typeof(ZeroWebMvcModule),
        typeof(AbpWebMvcModule)
        )]
    public class SampleWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<SampleNavigationProvider>();
        }

        public override void Initialize()
        {
            Clock.Provider = ClockProviders.Utc;
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component
                    .For<IAuthenticationManager>()
                    .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient()
            );

            

            ModelBinderProviders.BinderProviders.Add(new DataTableBinderProvider());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}