using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.ProjectName.EntityFramework;

namespace CompanyName.ProjectName.Tests
{
    public class ProjectNameInitialDataBuilder
    {
        public void Build(ProjectNameDbContext context)
        {
            //Add some people            
            //context.People.AddOrUpdate(
            //    p => p.Name,
            //    new Person { Name = "Isaac Asimov" },
            //    new Person { Name = "Thomas More" },
            //    new Person { Name = "George Orwell" },
            //    new Person { Name = "Douglas Adams" }
            //    );
            //context.SaveChanges();
        }
    }
}
