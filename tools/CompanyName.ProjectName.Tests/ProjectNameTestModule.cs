using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Modules;
using Abp.MultiTenancy;
using Abp.TestBase;
using Abp.Zero.Configuration;
using CompanyName.ProjectName;
using CompanyName.ProjectName.EntityFramework;
using Castle.MicroKernel.Registration;
using NSubstitute;

namespace CompanyName.ProjectName.Tests
{
    [DependsOn(
       typeof(ProjectNameEntityFrameworkModule),
       typeof(ProjectNameApplicationModule),
       typeof(AbpTestBaseModule)
       )]
    public class ProjectNameTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Registering fake services

            IocManager.IocContainer.Register(
                Component.For<IAbpZeroDbMigrator>()
                    .UsingFactoryMethod(() => Substitute.For<IAbpZeroDbMigrator>())
                    .LifestyleSingleton()
                );
        }
    }
}
