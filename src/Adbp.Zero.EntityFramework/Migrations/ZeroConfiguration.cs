using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using Adbp.Zero.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace Adbp.Zero.Migrations
{
    public class ZeroConfiguration<TDbContext> : DbMigrationsConfiguration<TDbContext>
        where TDbContext: DbContext
    {
        
        public ZeroConfiguration(string contextKey)
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = contextKey;
        }
        
        protected override void Seed(TDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.DisableAllFilters();
        }
    }
}
