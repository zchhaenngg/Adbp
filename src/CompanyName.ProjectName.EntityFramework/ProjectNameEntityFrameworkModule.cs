using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using CompanyName.ProjectName.EntityFramework;
using Adbp.Zero;

namespace CompanyName.ProjectName.EntityFramework
{
    [DependsOn(
        typeof(ProjectNameCoreModule),
        typeof(ZeroEntityFrameworkModule)
        )]
    public class ProjectNameEntityFrameworkModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ProjectNameDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.Register<IMetadataManager, MetadataManager>(Abp.Dependency.DependencyLifeStyle.Singleton);
        }
    }
}
