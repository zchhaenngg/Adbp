using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;

namespace Adbp.Sample.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : SampleControllerBase
    {
        protected override void OnException(ExceptionContext context)
        {
            base.OnException(context);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult JsonSuccess(string info)
        {
            return Json(new { Message = info });
        }

        [HttpPost]
        public JsonResult JsonError(string info)
        {
            throw new UserFriendlyException("error json");
        }

        [DontWrapResult]
        [HttpPost]
        public JsonResult JsonNotWrapped(string info)
        {
            return Json(new { });
        }
        
        [HttpPost]
        public ActionResult HtmlResult(string info)
        {
            return Content(info);
        }

        [HttpPost]
        public ActionResult Comment(string comment)
        {
            return Json(null);
        }
    }
}