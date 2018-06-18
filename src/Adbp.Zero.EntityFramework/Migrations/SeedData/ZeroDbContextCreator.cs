using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Editions;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
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

    public class ZeroDbContextCreator
    {
        protected TenantCreator TenantCreator { get; }
        protected LanguageCreator LanguageCreator { get; }
        protected EditionCreator EditionCreator { get; }
        protected RoleCreator RoleCreator { get; }
        protected UserCreator UserCreator { get; }
        protected SettingCreator SettingCreator { get; }

        public ZeroDbContextCreator(ZeroDbContext context)
        {
            TenantCreator = new TenantCreator(context);
            LanguageCreator = new LanguageCreator(context);
            EditionCreator = new EditionCreator(context);
            RoleCreator = new RoleCreator(context);
            UserCreator = new UserCreator(context);
            SettingCreator = new SettingCreator(context);
        }

        public void Create()
        {
            EditionCreator.Create();
            TenantCreator.Create();
            LanguageCreator.Create();
            RoleCreator.Create();
            UserCreator.Create();
            SettingCreator.Create();
        }
    }

    public class TenantCreator : ZeroDbContextCreatorBase
    {
        public TenantCreator(ZeroDbContext context)
            : base(context)
        {
        }

        internal void Create()
        {
            AddTenantIfNotExists(Tenant.DefaultTenantName);
        }
    }

    public class LanguageCreator : ZeroDbContextCreatorBase
    {
        public LanguageCreator(ZeroDbContext context)
            : base(context)
        {
        }

        internal void Create()
        {
            AddLanguageIfNotExists(null, "en", "English", "famfamfam-flag-gb");
            AddLanguageIfNotExists(null, "zh-CN", "简体中文", "famfamfam-flag-cn");
        }
    }

    public class EditionCreator : ZeroDbContextCreatorBase
    {
        public EditionCreator(ZeroDbContext context)
            : base(context)
        {
        }

        internal void Create()
        {
            AddEditionIfNotExists(EditionManager.DefaultEditionName);
        }
    }

    public class RoleCreator : ZeroDbContextCreatorBase
    {
        public static List<AuthorizationProvider> AuthorizationProviders;
        static RoleCreator()
        {
            AuthorizationProviders = new List<AuthorizationProvider> {
                new ZeroAuthorizationProvider()
            };
        }
        public RoleCreator(ZeroDbContext context)
            : base(context)
        {
            
        }

        internal void Create()
        {
            CreateAndGrantForHostAdminRole();
            CreateAndGrantForDefaultTenantAdminRole();
        }

        private void CreateAndGrantForHostAdminRole()
        {
            var role = AddRoleIfNotExists(null, StaticRoleNames.Host.Admin);
            var permissions = PermissionFinder.GetAllPermissions(AuthorizationProviders.ToArray())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host))
                .ToList();
            AddRolePermissionIfNotExists(role, permissions);
        }

        private void CreateAndGrantForDefaultTenantAdminRole()
        {
            var tenant = GetTenant(Tenant.DefaultTenantName);
            var role = AddRoleIfNotExists(tenant.Id, StaticRoleNames.Tenants.Admin);
            var permissions = PermissionFinder.GetAllPermissions(AuthorizationProviders.ToArray())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant))
                .ToList();
            AddRolePermissionIfNotExists(role, permissions);
        }

    }

    public class UserCreator : ZeroDbContextCreatorBase
    {
        public UserCreator(ZeroDbContext context)
            : base(context)
        {
        }

        internal void Create()
        {
            CreateAndGrantForHostAdminUser();
            CreateAndGrantForDefaultTenantAdminUser();
        }

        private void CreateAndGrantForHostAdminUser()
        {
            var role = GetRole(null, StaticRoleNames.Host.Admin);
            var user = AddUserIfNotExists(null, User.AdminUserName, "System", "Administrator", "adminHost@adbp.com");
            AddUserRoleIfNotExists(null, role, user);
        }

        private void CreateAndGrantForDefaultTenantAdminUser()
        {
            var tenant = GetTenant(Tenant.DefaultTenantName);
            var role = GetRole(tenant.Id, StaticRoleNames.Host.Admin);
            var user = AddUserIfNotExists(tenant.Id, User.AdminUserName, "System", "Administrator", "adminDefault@adbp.com");
            AddUserRoleIfNotExists(tenant.Id, role, user);
        }
    }

    public class SettingCreator : ZeroDbContextCreatorBase
    {
        public SettingCreator(ZeroDbContext context)
            : base(context)
        {
        }

        internal void Create()
        {
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@mydomain.com");
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "mydomain.com mailer");

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en");
        }
    }
}
