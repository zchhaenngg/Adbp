using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adbp.Zero.EntityFramework;

namespace Adbp.Sample.EntityFramework
{
    public class MetadataManager : MetadataManagerBase
    {
        public MetadataManager(SampleDbContext dbContext) : base(dbContext)
        {
        }
    }
}
