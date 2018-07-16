using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Adbp.Zero.Authorization;
using Adbp.Zero.Configuration;
using Adbp.Zero.Configuration.Dto;

namespace Adbp.Zero.MVC.Controllers
{
    [AbpMvcAuthorize]
    public class ZeroSystemController : ZeroControllerBase
    {
        private readonly IConfigurationAppService _configurationAppService;

        public ZeroSystemController(
            IConfigurationAppService configurationAppService)
        {
            _configurationAppService = configurationAppService;
        }

        [AbpMvcAuthorize(ZeroPermissionNames.Permissions_SystemSetting)]
        public ActionResult Settings()
        {
            ViewBag.TenantId = AbpSession.TenantId;
            if (AbpSession.TenantId == null)
            {
                ViewBag.SettingDefinitions = _configurationAppService.GetAllSettingDefinitionsForApplication();
            }
            else
            {
                ViewBag.SettingDefinitions = _configurationAppService.GetAllSettingDefinitionsForTenant();
            }
            return View();
        }
    }
}