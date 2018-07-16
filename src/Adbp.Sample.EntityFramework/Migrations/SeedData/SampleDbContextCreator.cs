using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Net.Mail;
using Abp.Zero.Ldap.Configuration;
using Adbp.Sample.Authorization;
using Adbp.Sample.EntityFramework;
using Adbp.Zero.EntityFramework;
using Adbp.Zero.Migrations.SeedData;

namespace Adbp.Sample.Migrations.SeedData
{
    public class SampleDbContextCreator: ZeroDbContextCreator
    {
        public SampleDbContextCreator(SampleDbContext context)
            :base(context)
        {
            SeedDataConfig.AuthorizationProviders.Add(new SampleAuthorizationProvider());
        }
        public void Create()
        {
            base.Init();
            new SettingCreateor(DbContext, SampleConsts.DefaultTenantId).Create();
            new OrganizationUnitCreator(DbContext, SampleConsts.DefaultTenantId).Create();
        }
    }

    internal class SettingCreateor : SampleDbContextCreatorBase
    {
        private readonly int? _tenantId;

        public SettingCreateor(ZeroDbContext context, int? tenantId)
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

    internal class OrganizationUnitCreator : SampleDbContextCreatorBase
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
