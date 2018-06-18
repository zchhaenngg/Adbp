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
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Users;
using Adbp.Zero.EntityFramework;
using Adbp.Zero.MultiTenancy;

namespace Adbp.Zero.Migrations.SeedData
{
    public abstract class ZeroDbContextCreatorBase
    {
        public ZeroDbContext Context { get; }
        public ZeroDbContextCreatorBase(ZeroDbContext context)
        {

            Context = context;
        }

        #region tenant
        protected Tenant GetTenant(string name)
        {
            return Context.Tenants.SingleOrDefault(x => x.TenancyName == Tenant.DefaultTenantName);
        }

        protected void AddTenantIfNotExists(string tenancyName, string display = null)
        {
            if (!Context.Tenants.Any(x => x.TenancyName == tenancyName))
            {
                Context.Tenants.Add(new Tenant
                {
                    TenancyName = tenancyName,
                    Name = display ?? tenancyName
                });
                Context.SaveChanges();
            }
        }
        #endregion

        #region language
        protected void AddLanguageIfNotExists(int? tenantId, string name, string displayName, string icon = null, bool isDisabled = false)
        {
            if (!Context.Languages.Any(x => x.TenantId == tenantId && x.Name == name))
            {
                Context.Languages.Add(new ApplicationLanguage(tenantId, name, displayName, icon, isDisabled));
                Context.SaveChanges();
            }
        }
        #endregion

        #region edition
        protected void AddEditionIfNotExists(string name, string display = null)
        {
            if (!Context.Editions.Any(x => x.Name == name))
            {
                Context.Editions.Add(new Edition
                {
                    Name = name,
                    DisplayName = display ?? name
                });
                Context.SaveChanges();
            }
        }
        #endregion

        #region role
        protected Role AddRoleIfNotExists(int? tenantId, string name, string display = null, bool isStatic = true)
        {
            if (!Context.Roles.Any(x => x.TenantId == tenantId && x.Name == name))
            {
                Context.Roles.Add(new Role
                {
                    TenantId = tenantId,
                    Name = name,
                    DisplayName = display ?? name,
                    IsStatic = isStatic
                });
                Context.SaveChanges();
            }
            return GetRole(tenantId, name);
        }

        protected Role GetRole(int? tenantId, string roleName)
        {
            return Context.Roles.SingleOrDefault(x => x.TenantId == tenantId && x.Name == roleName);
        }
        #endregion
        
        #region user
        protected User AddUserIfNotExists(int? tenantId, string username, string name, string surname, string email, string password = User.DefaultPassword)
        {
            if (!Context.Users.Any(x => x.TenantId == tenantId && x.UserName == username))
            {
                Context.Users.Add(new User
                {
                    TenantId = tenantId,
                    UserName = username,
                    Name = name,
                    Surname = surname,
                    EmailAddress = email,
                    Password = new Microsoft.AspNet.Identity.PasswordHasher().HashPassword(password),
                    IsActive = true,
                    IsEmailConfirmed = true
                });
                Context.SaveChanges();
            }
            return GetUser(tenantId, username);
        }

        protected User GetUser(int? tenantId, string username)
        {
            return Context.Users.SingleOrDefault(x => x.TenantId == tenantId && x.UserName == username);
        }
        #endregion

        #region grant
        protected void AddRolePermissionIfNotExists(Role role, List<Permission> permissions)
        {
            foreach (var permission in permissions)
            {
                if (!Context.RolePermissions.Any(x => x.TenantId == role.TenantId && x.RoleId == role.Id && x.Name == permission.Name))
                {
                    Context.RolePermissions.Add(new RolePermissionSetting
                    {
                        TenantId = role.TenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = role.Id
                    });
                    Context.SaveChanges();
                }
            }
        }

        protected void AddUserRoleIfNotExists(int? tenantId, Role role, params User[] users)
        {
            foreach (var user in users)
            {
                if (!Context.UserRoles.Any(x => x.TenantId == tenantId && x.RoleId == role.Id && x.UserId == user.Id))
                {
                    Context.UserRoles.Add(new UserRole(tenantId, user.Id, role.Id));
                    Context.SaveChanges();
                }
            }
        }
        #endregion

        #region settings
        protected void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (Context.Settings.Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            Context.Settings.Add(new Setting(tenantId, null, name, value));
            Context.SaveChanges();
        }
        #endregion
    }
}
