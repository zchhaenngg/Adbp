using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.UI;
using Adbp.Zero.MVC.Controllers;

namespace CompanyName.ProjectName.Web.Controllers
{
    public class ProjectNameControllerBase : ZeroControllerBase
    {
        protected ProjectNameControllerBase()
        {
            LocalizationSourceName = ProjectNameConsts.LocalizationSourceName;
        }
        
    }
}