using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Adbp.Sample.EntityFramework;
using Adbp.Zero;

namespace Adbp.Sample.EntityFramework
{
    [DependsOn(
        typeof(ZeroDataModule),
        typeof(SampleCoreModule)
        )]
    public class SampleDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<SampleDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.Register<IMetadataManager, MetadataManager>(Abp.Dependency.DependencyLifeStyle.Singleton);
        }
    }
}
