namespace Adbp.Zero.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Abp.Zero.EntityFramework;
    using Adbp.Zero.SysObjectSettings;
    using Adbp.Zero.Authorization.Roles;
    using Adbp.Zero.Authorization.Users;
    using Adbp.Zero.MultiTenancy;
    using Abp.Organizations;
    using Adbp.Zero.OrganizationUnits;
    using Adbp.Zero.Emails;
    
    public abstract class ZeroDbContext : ZeroDbContext<Tenant, Role, User>
    {
        public ZeroDbContext(string name)
            : base(name)
        {
        }
    }
    
    public abstract class ZeroDbContext<TTenant, TRole, TUser> : AbpZeroDbContext<TTenant, TRole, TUser>
        where TTenant : Tenant<TUser>
        where TRole : Role<TUser>
        where TUser : User<TUser>
    {
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        public virtual IDbSet<ZeroOrganizationUnit> AdbpOrganizationUnits { get; set; }

        public virtual IDbSet<SysObjectSetting> SysObjectSettings { get; set; }

        public virtual IDbSet<Email> Emails { get; set; }
        // Your context has been configured to use a 'SampleDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Adbp.Sample.EntityFramework.EntityFramework.SampleDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SampleDbContext' 
        // connection string in the application configuration file.
        public ZeroDbContext(string name)
            : base(name)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // overide all abp
            modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>("Abp_");
            // 和OrganizationUnit表名保持一致
            modelBuilder.Entity<ZeroOrganizationUnit>().ToTable("Abp_OrganizationUnits");

            //adbp
            SetAdbpTableName<SysObjectSetting>(modelBuilder);
            SetAdbpTableName<Email>(modelBuilder);
        }

        private void SetAdbpTableName<TEntity>(DbModelBuilder modelBuilder)
             where TEntity : class
        {
            SetTableName<TEntity>(modelBuilder, "Adbp_");
        }

        protected void SetTableName<TEntity>(DbModelBuilder modelBuilder, string prefix)
            where TEntity : class
        {
            modelBuilder.Entity<TEntity>().ToTable($"{prefix}{typeof(TEntity).Name}");
        }
    }
}