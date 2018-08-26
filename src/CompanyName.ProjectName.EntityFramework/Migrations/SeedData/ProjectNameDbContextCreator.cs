using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Net.Mail;
using Abp.Zero.Ldap.Configuration;
using CompanyName.ProjectName.Authorization;
using CompanyName.ProjectName.EntityFramework;
using Adbp.Zero.EntityFramework;
using Adbp.Zero.Migrations.SeedData;

namespace CompanyName.ProjectName.Migrations.SeedData
{
    public class ProjectNameDbContextCreator: ZeroDbContextCreator
    {
        public ProjectNameDbContextCreator(ProjectNameDbContext context)
            :base(context)
        {
            SeedDataConfig.AuthorizationProviders.Add(new ProjectNameAuthorizationProvider());
        }
        public void Create()
        {
            base.Init();
            new SettingCreator(DbContext, ProjectNameConsts.DefaultTenantId).Create();
            new OrganizationUnitCreator(DbContext, ProjectNameConsts.DefaultTenantId).Create();
        }
    }

    internal class SettingCreator : ProjectNameDbContextCreatorBase
    {
        private readonly int? _tenantId;

        public SettingCreator(ZeroDbContext context, int? tenantId)
             : base(context)
        {
            _tenantId = tenantId;
        }
        internal void Create()
        {
            // LDAP
            AddSettingIfNotExists(LdapSettingNames.IsEnabled, true.ToString(), _tenantId);

            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "xxxxx@xxx.com", _tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "xxx mailer", _tenantId);

            AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "", _tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Port, "25", _tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, "", _tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Password, "", _tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Domain, "", _tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl, "false", _tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, "false", _tenantId);
        }
    }

    internal class OrganizationUnitCreator : ProjectNameDbContextCreatorBase
    {
        private readonly int? _tenantId;

        public OrganizationUnitCreator(ZeroDbContext context, int? tenantId)
            : base(context)
        {
            _tenantId = tenantId;
        }

        internal void Create()
        {
            AddOrganizationIfNotExists(_tenantId, "static test", "this is a static ou", true, 1);
        }
    }
}
