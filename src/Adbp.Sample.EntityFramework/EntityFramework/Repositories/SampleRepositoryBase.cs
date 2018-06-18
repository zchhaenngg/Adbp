using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using Adbp.Sample.EntityFramework;
using Adbp.Zero.EntityFramework.Repositories;

namespace Adbp.Sample.EntityFramework.Repositories
{
    public abstract class SampleRepositoryBase<TEntity, TPrimaryKey> : ZeroRepositoryBase<SampleDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected SampleRepositoryBase(IDbContextProvider<SampleDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class SampleRepositoryBase<TEntity> : SampleRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected SampleRepositoryBase(IDbContextProvider<SampleDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
