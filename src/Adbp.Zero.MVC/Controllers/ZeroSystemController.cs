using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Adbp.Zero.Authorization;

namespace Adbp.Zero.MVC.Controllers
{
    [AbpMvcAuthorize]
    public class ZeroSystemController : Controller
    {
        [AbpMvcAuthorize(PermissionNames.Permissions_SystemSetting)]
        public ActionResult Settings()
        {
            return View();
        }
    }
}