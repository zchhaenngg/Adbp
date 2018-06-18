namespace Adbp.Sample.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Abp.Zero.EntityFramework;
    using Adbp.Sample.Contacts;
    using Adbp.Sample.Guests;
    using Adbp.Zero.Authorization.Roles;
    using Adbp.Zero.Authorization.Users;
    using Adbp.Zero.EntityFramework;
    using Adbp.Zero.MultiTenancy;

    public class SampleDbContext : ZeroDbContext
    {
        // Your context has been configured to use a 'SampleDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Adbp.Sample.EntityFramework.EntityFramework.SampleDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SampleDbContext' 
        // connection string in the application configuration file.
        public SampleDbContext()
            : base("name=Default")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual IDbSet<Guest> Guests { get; set; }
        public virtual IDbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SetTableName<Guest>(modelBuilder);
            SetTableName<Contact>(modelBuilder);
        }

        private void SetTableName<TEntity>(DbModelBuilder modelBuilder)
             where TEntity : class
        {
            SetTableName<TEntity>(modelBuilder, AdbpConsts.TableConsts.CUSTOM);
        }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}