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
using Abp.Organizations;
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Users;
using Adbp.Zero.EntityFramework;
using Adbp.Zero.MultiTenancy;

namespace Adbp.Zero.Migrations.SeedData
{
    public abstract class ZeroDbContextCreatorBase : ZeroDbContextCreatorBase<Tenant, Role, User>
    {
        public ZeroDbContextCreatorBase(ZeroDbContext<Tenant, Role, User> context) 
            : base(context)
        {
        }
    }

    public abstract class ZeroDbContextCreatorBase<TTenant, TRole, TUser>
        where TTenant : Tenant<TUser>, new()
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>, new()
    {
        public ZeroDbContext<TTenant, TRole, TUser> Context { get; }
        public ZeroDbContextCreatorBase(ZeroDbContext<TTenant, TRole, TUser> context)
        {
            Context = context;
        }

        #region tenant
        protected TTenant GetTenant(string name)
        {
            return Context.Tenants.SingleOrDefault(x => x.TenancyName == Tenant.DefaultTenantName);
        }

        protected void AddTenantIfNotExists(string tenancyName, string display = null)
        {
            if (!Context.Tenants.Any(x => x.TenancyName == tenancyName))
            {
                Context.Tenants.Add(new TTenant
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
        protected TRole AddRoleIfNotExists(int? tenantId, string name, string display = null, bool isStatic = true)
        {
            if (!Context.Roles.Any(x => x.TenantId == tenantId && x.Name == name))
            {
                Context.Roles.Add(new TRole
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

        protected TRole GetRole(int? tenantId, string roleName)
        {
            return Context.Roles.SingleOrDefault(x => x.TenantId == tenantId && x.Name == roleName);
        }
        #endregion
        
        #region user
        protected TUser AddUserIfNotExists(int? tenantId, string username, string name, string surname, string email, string password = User.DefaultPassword)
        {
            if (!Context.Users.Any(x => x.TenantId == tenantId && x.UserName == username))
            {
                Context.Users.Add(new TUser
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

        protected TUser GetUser(int? tenantId, string username)
        {
            return Context.Users.SingleOrDefault(x => x.TenantId == tenantId && x.UserName == username);
        }
        #endregion

        #region grant
        protected void AddRolePermissionIfNotExists(TRole role, List<Permission> permissions)
        {
            foreach (var permission in permissions)
            {
                if (!Context.RolePermissions.Any(x => x.RoleId == role.Id && x.Name == permission.Name))
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

        protected void AddUserRoleIfNotExists(TRole role, params TUser[] users)
        {
            foreach (var user in users)
            {
                if (!Context.UserRoles.Any(x => x.RoleId == role.Id && x.UserId == user.Id))
                {
                    Context.UserRoles.Add(new UserRole(role.TenantId, user.Id, role.Id));
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

        #region organizationUnits
        protected void AddOrganizationIfNotExists(int? tenantId, string groupCode, string displayName, bool isStatic, params int[] numbers)
        {
            if (!Context.ZeroOrganizationUnits.Any(x => x.GroupCode == groupCode && x.DisplayName == displayName))
            {
                Context.ZeroOrganizationUnits.Add(new Adbp.Zero.OrganizationUnits.ZeroOrganizationUnit
                {
                    TenantId = tenantId,
                    GroupCode = groupCode,
                    DisplayName = displayName,
                    Code = OrganizationUnit.AppendCode(null, OrganizationUnit.CreateCode(numbers)),
                    IsStatic = isStatic
                });

                Context.SaveChanges();
            }
        }
        #endregion
    }
}
