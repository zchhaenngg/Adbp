using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using Adbp.Zero.EntityFramework;

namespace Adbp.Zero.EntityFramework.Repositories
{
    public abstract class ZeroRepositoryBase<TDbContext, TEntity, TPrimaryKey> : EfRepositoryBase<TDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext: ZeroDbContext
    {
        protected ZeroRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class ZeroRepositoryBase<TDbContext, TEntity> : ZeroRepositoryBase<TDbContext, TEntity, int>
        where TEntity : class, IEntity<int>
        where TDbContext : ZeroDbContext
    {
        protected ZeroRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
