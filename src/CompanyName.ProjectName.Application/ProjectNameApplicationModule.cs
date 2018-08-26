using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Threading.BackgroundWorkers;
using Adbp.Zero;

namespace CompanyName.ProjectName
{
    [DependsOn(
        typeof(ProjectNameCoreModule),
        typeof(ZeroApplicationModule))]
    public class ProjectNameApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();

            Configuration.Modules.ZeroApplicationModule().IsEmailWorkerEnabled = false;
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            // TODO: Is there somewhere else to store these, with the dto classes
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
               
            });
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            }
        }
    }
}
