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
using CompanyName.ProjectName.EntityFramework;
using CompanyName.ProjectName.Api;
using Abp.Zero.Configuration;
using Abp.Timing;
using Adbp.Zero.MVC.App_Start;
using Adbp.Zero.MVC;
using Abp.Web.SignalR;

namespace CompanyName.ProjectName.Web.App_Start
{
    [DependsOn(
        typeof(ProjectNameEntityFrameworkModule),
        typeof(ProjectNameApplicationModule),
        typeof(ProjectNameWebApiModule),
        typeof(AbpWebSignalRModule),
        //typeof(AbpHangfireModule), - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        typeof(ZeroWebMvcModule)
        )]
    public class ProjectNameWebModule : AbpModule
    {
        public override void PreInitialize()
        {
#if DEBUG
            StaticResourceConfig.AdbpJsPath = "~/AdbpJs";

#endif
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<ProjectNameNavigationProvider>();

            Configuration.BackgroundJobs.IsJobExecutionEnabled = AppEnvHelper.IsJobExecutionEnabled();

            IocManager.Register<BundleConfig>();
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

            IocManager.Resolve<BundleConfig>().RegisterBundles(BundleTable.Bundles);
        }
    }
}