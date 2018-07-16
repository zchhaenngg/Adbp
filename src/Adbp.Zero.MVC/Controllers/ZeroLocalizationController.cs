using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Localization;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Adbp.Linq.Expressions;
using Adbp.Paging;
using Adbp.Paging.Dto;
using Adbp.Zero.Authorization;
using Adbp.Zero.Localization;
using Adbp.Zero.Localization.Dto;

namespace Adbp.Zero.MVC.Controllers
{
    [AbpMvcAuthorize(ZeroPermissionNames.Permissions_ApplicationLanguageText)]
    public class ZeroLocalizationController : ZeroControllerBase
    {
        private readonly IApplicationLanguageTextAppService _applicationLanguageTextAppService;

        public ZeroLocalizationController(
            IApplicationLanguageTextAppService applicationLanguageTextAppService)
        {
            _applicationLanguageTextAppService = applicationLanguageTextAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetAllLocalizedStrings()
        {
            var items = _applicationLanguageTextAppService.GetAllLocalizedStringDtos();
            var page = new PagedResultDto<LocalizedStringDto>(items.Count, items);
            return Json(page);
        }
    }
}
