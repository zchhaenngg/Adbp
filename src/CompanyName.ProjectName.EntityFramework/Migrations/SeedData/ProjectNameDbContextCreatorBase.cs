using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Organizations;
using CompanyName.ProjectName.EntityFramework;
using Adbp.Zero.EntityFramework;
using Adbp.Zero.Migrations.SeedData;

namespace CompanyName.ProjectName.Migrations.SeedData
{
    internal class ProjectNameDbContextCreatorBase : ZeroDbContextCreatorBase
    {
        public ProjectNameDbContextCreatorBase(ZeroDbContext context) : base(context)
        {
        }
    }
}
