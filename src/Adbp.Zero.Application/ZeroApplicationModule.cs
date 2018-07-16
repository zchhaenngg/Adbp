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
using Abp.Organizations;
using Adbp.Zero.Authorization.Roles.Dto;
using Adbp.Zero.OrganizationUnits.Dto;
using Adbp.Zero.Users.Dto;
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Users;
using Abp.Threading.BackgroundWorkers;
using Adbp.Zero.BackgroundWorkers;

namespace Adbp.Zero
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(ZeroCoreModule)
        )]
    public class ZeroApplicationModule: AbpModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();
            IocManager.Register<ZeroApplicationModuleConfig>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            // TODO: Is there somewhere else to store these, with the dto classes
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                // Role and permission
                cfg.CreateMap<Permission, string>().ConvertUsing(r => r.Name);
                cfg.CreateMap<RolePermissionSetting, string>().ConvertUsing(r => r.Name);

                cfg.CreateMap<CreateRoleDto, Role>().ForMember(x => x.Permissions, opt => opt.Ignore());
                cfg.CreateMap<UpdateRoleDto, Role>().ForMember(x => x.Permissions, opt => opt.Ignore());
                cfg.CreateMap<RoleDto, Role>().ForMember(x => x.Permissions, opt => opt.Ignore());

                cfg.CreateMap<User, UserDto>().ForMember(x => x.RoleIds, opt => opt.MapFrom(x => x.Roles.Select(r => r.RoleId)));
                cfg.CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

                cfg.CreateMap<OrganizationUnit, OrganizationUnitOutput>().ForMember(x => x.MemberCount, opt => opt.MapFrom(x => x.Children.Count));
            });
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
                if (Configuration.Get<ZeroApplicationModuleConfig>().IsEmailWorkerEnabled)
                {
                    workManager.Add(IocManager.Resolve<EmailWorker>());
                }
            }
        }
    }
}
