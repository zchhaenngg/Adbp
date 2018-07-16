using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.EntityFramework;
using Adbp.Zero;
using Adbp.Zero.EntityFramework;
using Adbp.Zero.EntityFramework.Repositories;

namespace Adbp.Zero
{
    [DependsOn(
        typeof(AbpZeroEntityFrameworkModule)
        )]
    public class ZeroEntityFrameworkModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<ZeroDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
