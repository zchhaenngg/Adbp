using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace Adbp.Zero.MVC.Controllers
{
    [AbpMvcAuthorize]
    public class ZeroDevController : ZeroControllerBase
    {
        public ZeroDevController()
        {

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
