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
    using System.Data.Common;

    public abstract class ZeroDbContext : ZeroDbContext<Tenant, Role, User>
    {
        public ZeroDbContext(string name)
            : base(name)
        {
        }
        
        public ZeroDbContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
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
        public virtual IDbSet<ZeroOrganizationUnit> ZeroOrganizationUnits { get; set; }
        public virtual IDbSet<ZeroUserOrganizationUnit> ZeroUserOrganizationUnits { get; set; }

        public virtual IDbSet<SysObjectSetting> SysObjectSettings { get; set; }

        public virtual IDbSet<LoginAgent> LoginAgents { get; set; }

        public virtual IDbSet<Email> Emails { get; set; }
        // Your context has been configured to use a 'SampleDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CompanyName.ProjectNameEntityFramework.EntityFramework.SampleDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SampleDbContext' 
        // connection string in the application configuration file.
        public ZeroDbContext(string name)
            : base(name)
        {
        }

        //This constructor is used in tests
        public ZeroDbContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LoginAgent>().HasRequired(x => x.Principal).WithMany().HasForeignKey(f => f.PrincipalId).WillCascadeOnDelete(false);
            modelBuilder.Entity<LoginAgent>().HasRequired(x => x.Agent).WithMany().HasForeignKey(f => f.AgentId).WillCascadeOnDelete(false);

            // overide all abp
            modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>("Abp_");
            // 和OrganizationUnit表名保持一致
            modelBuilder.Entity<ZeroOrganizationUnit>().ToTable("Abp_OrganizationUnits");
            modelBuilder.Entity<ZeroUserOrganizationUnit>().ToTable("Abp_UserOrganizationUnits");
            //adbp
            SetAdbpTableName<SysObjectSetting>(modelBuilder);
            SetAdbpTableName<Email>(modelBuilder);
            SetAdbpTableName<LoginAgent>(modelBuilder);
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