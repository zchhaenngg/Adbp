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

namespace Adbp.Sample.Web.Controllers
{
    public class SampleControllerBase : ZeroControllerBase
    {
        protected SampleControllerBase()
        {
            LocalizationSourceName = SampleConsts.LocalizationSourceName;
        }
        
    }
}