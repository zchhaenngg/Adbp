using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using Adbp.Sample.Authorization;
using Adbp.Zero.Migrations;
using Adbp.Zero.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace Adbp.Sample.Migrations
{
    public sealed class Configuration : ZeroConfiguration<Adbp.Sample.EntityFramework.SampleDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }
        public Configuration()
            :base("AdbpSample")
        {

        }
        
        protected override void Seed(Adbp.Sample.EntityFramework.SampleDbContext context)
        {
            RoleCreator.AuthorizationProviders.Add(new SampleAuthorizationProvider());
            base.Seed(context);
        }
    }
}
