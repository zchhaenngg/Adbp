using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using CompanyName.ProjectName.EntityFramework;
using Adbp.Zero.EntityFramework.Repositories;

namespace CompanyName.ProjectName.EntityFramework.Repositories
{
    public abstract class ProjectNameRepositoryBase<TEntity, TPrimaryKey> : ZeroRepositoryBase<ProjectNameDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected ProjectNameRepositoryBase(IDbContextProvider<ProjectNameDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class ProjectNameRepositoryBase<TEntity> : ProjectNameRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected ProjectNameRepositoryBase(IDbContextProvider<ProjectNameDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
