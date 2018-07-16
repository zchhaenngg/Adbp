using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Organizations;
using Adbp.Sample.EntityFramework;
using Adbp.Zero.EntityFramework;
using Adbp.Zero.Migrations.SeedData;

namespace Adbp.Sample.Migrations.SeedData
{
    internal class SampleDbContextCreatorBase : ZeroDbContextCreatorBase
    {
        public SampleDbContextCreatorBase(ZeroDbContext context) : base(context)
        {
        }
    }
}
