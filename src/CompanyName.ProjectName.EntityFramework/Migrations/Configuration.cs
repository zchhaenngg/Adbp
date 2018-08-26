using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using CompanyName.ProjectName.Authorization;
using CompanyName.ProjectName.Migrations.SeedData;
using Adbp.Zero.Migrations;
using Adbp.Zero.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace CompanyName.ProjectName.Migrations
{
    public sealed class Configuration : ZeroConfiguration<CompanyName.ProjectName.EntityFramework.ProjectNameDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }
        public Configuration()
            :base("AdbpSample")
        {

        }
        
        protected override void Seed(CompanyName.ProjectName.EntityFramework.ProjectNameDbContext context)
        {
            Init(context);
        }

        public void Init(CompanyName.ProjectName.EntityFramework.ProjectNameDbContext context)
        {
            base.Seed(context);
            new ProjectNameDbContextCreator(context).Create();
        }
    }
}
