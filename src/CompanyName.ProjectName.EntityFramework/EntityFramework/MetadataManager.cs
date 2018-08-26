using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adbp.Zero.EntityFramework;

namespace CompanyName.ProjectName.EntityFramework
{
    public class MetadataManager : MetadataManagerBase
    {
        public MetadataManager(ProjectNameDbContext dbContext) : base(dbContext)
        {
        }
    }
}
