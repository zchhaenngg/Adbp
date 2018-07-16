using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Editions;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Collections;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Adbp.Zero.Authorization;
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Users;
using Adbp.Zero.Editions;
using Adbp.Zero.EntityFramework;
using Adbp.Zero.MultiTenancy;

namespace Adbp.Zero.Migrations.SeedData
{
    public abstract class ZeroDbContextCreator : ZeroDbContextCreator<ZeroDbContext>
    {
        protected ZeroDbContextCreator(ZeroDbContext context) 
            : base(context)
        {
        }
    }

    public abstract class ZeroDbContextCreator<TZeroDbContext> : ZeroDbContextCreator<Tenant, Role, User, TZeroDbContext>
        where TZeroDbContext : ZeroDbContext
    {
        protected ZeroDbContextCreator(TZeroDbContext context) 
            : base(context)
        {
        }
    }
    public abstract class ZeroDbContextCreator<TTenant, TRole, TUser, TZeroDbContext>
        where TTenant : Tenant<TUser>, new()
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>, new()
        where TZeroDbContext: ZeroDbContext<TTenant, TRole, TUser>
    {
        protected TZeroDbContext DbContext { get; private set; }

        protected ZeroDbContextCreator(TZeroDbContext context)
        {
            DbContext = context;
        }

        protected virtual void Init()
        {
            new EditionCreator<TTenant, TRole, TUser>(DbContext).Create();
            new TenantCreator<TTenant, TRole, TUser>(DbContext).Create();
            new LanguageCreator<TTenant, TRole, TUser>(DbContext, null).Create();
            new RoleCreator<TTenant, TRole, TUser>(DbContext, null).Create();
            new RoleCreator<TTenant, TRole, TUser>(DbContext, ZeroConsts.DefaultTenantId).Create();
            new UserCreator<TTenant, TRole, TUser>(DbContext, null).Create();
            new UserCreator<TTenant, TRole, TUser>(DbContext, ZeroConsts.DefaultTenantId).Create();
            new SettingCreator<TTenant, TRole, TUser>(DbContext, null).Create();
        }
    }

    internal class TenantCreator<TTenant, TRole, TUser> : ZeroDbContextCreatorBase<TTenant, TRole, TUser>
        where TTenant : Tenant<TUser>, new()
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>, new()
    {
        public TenantCreator(ZeroDbContext<TTenant, TRole, TUser> context)
            : base(context)
        {
        }

        internal void Create()
        {
            AddTenantIfNotExists(Tenant.DefaultTenantName);
        }
    }

    internal class LanguageCreator<TTenant, TRole, TUser> : ZeroDbContextCreatorBase<TTenant, TRole, TUser>
        where TTenant : Tenant<TUser>, new()
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>, new()
    {
        private readonly int? _tenantId;

        public LanguageCreator(ZeroDbContext<TTenant, TRole, TUser> context, int? tenantId)
            : base(context)
        {
            _tenantId = tenantId;
        }

        internal void Create()
        {
            AddLanguageIfNotExists(_tenantId, "en", "English", "famfamfam-flag-gb");
            //AddLanguageIfNotExists(_tenantId, "zh-CN", "简体中文", "famfamfam-flag-cn");
            AddLanguageIfNotExists(_tenantId, "zh-Hans", "简体中文", "famfamfam-flag-cn");
        }
    }

    internal class EditionCreator<TTenant, TRole, TUser> : ZeroDbContextCreatorBase<TTenant, TRole, TUser>
        where TTenant : Tenant<TUser>, new()
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>, new()
    {
        public EditionCreator(ZeroDbContext<TTenant, TRole, TUser> context)
            : base(context)
        {
        }

        internal void Create()
        {
            AddEditionIfNotExists(EditionManager.DefaultEditionName);
        }
    }

    internal class RoleCreator<TTenant, TRole, TUser> : ZeroDbContextCreatorBase<TTenant, TRole, TUser>
        where TTenant : Tenant<TUser>, new()
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>, new()
    {
        private readonly int? _tenantId;
        
        public RoleCreator(ZeroDbContext<TTenant, TRole, TUser> context, int? tenantId)
            : base(context)
        {
            _tenantId = tenantId;
        }

        internal void Create()
        {
            CreateAndGrantForAdmin();
        }

        private void CreateAndGrantForAdmin()
        {
            if (_tenantId == null)
            {
                var role = AddRoleIfNotExists(null, ZeroStaticRoleNames.Host.Admin);
                var permissions = PermissionFinder.GetAllPermissions(SeedDataConfig.AuthorizationProviders.ToArray())
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host))
                    .ToList();
                AddRolePermissionIfNotExists(role, permissions);
            }
            else
            {
                var role = AddRoleIfNotExists(_tenantId, ZeroStaticRoleNames.Tenants.Admin);
                var permissions = PermissionFinder.GetAllPermissions(SeedDataConfig.AuthorizationProviders.ToArray())
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant))
                    .ToList();
                AddRolePermissionIfNotExists(role, permissions);
            }
        }
    }

    internal class UserCreator<TTenant, TRole, TUser> : ZeroDbContextCreatorBase<TTenant, TRole, TUser>
        where TTenant : Tenant<TUser>, new()
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>, new()
    {
        private readonly int? _tenantId;

        public UserCreator(ZeroDbContext<TTenant, TRole, TUser> context, int? tenantId)
            : base(context)
        {
            _tenantId = tenantId;
        }

        internal void Create()
        {
            CreateAndGrantForAdmin();
        }

        private void CreateAndGrantForAdmin()
        {
            if (_tenantId == null)
            {
                var role = GetRole(null, ZeroStaticRoleNames.Host.Admin);
                var user = AddUserIfNotExists(null, User.AdminUserName, "System", "Administrator", "adminHost@adbp.com");
                AddUserRoleIfNotExists(role, user);
            }
            else
            {
                var role = GetRole(_tenantId, ZeroStaticRoleNames.Host.Admin);
                var user = AddUserIfNotExists(_tenantId, User.AdminUserName, "System", "Administrator", "adminDefault@adbp.com");
                AddUserRoleIfNotExists(role, user);
            }
        }
    }

    internal class SettingCreator<TTenant, TRole, TUser> : ZeroDbContextCreatorBase<TTenant, TRole, TUser>
        where TTenant : Tenant<TUser>, new()
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>, new()
    {
        private readonly int? _tenantId;

        public SettingCreator(ZeroDbContext<TTenant, TRole, TUser> context, int? tenantId)
            : base(context)
        {
            _tenantId = tenantId;
        }

        internal void Create()
        {
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@mydomain.com", _tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "mydomain.com mailer", _tenantId);

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", _tenantId);
        }
    }
}
