using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Adbp.Zero.SysObjectSettings;
using Adbp.Zero.Authorization;
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Roles.Dto;
using Adbp.Extensions;

namespace Adbp.Zero.MVC.Controllers
{
    [AbpMvcAuthorize(ZeroPermissionNames.Permissions_Role)]
    public class ZeroRolesController: ZeroControllerBase
    {
        private readonly IRoleAppService _roleAppService;
        private readonly ISysObjectSettingAppService _asysObjectSettingAppService;

        public ZeroRolesController(
            IRoleAppService roleAppService,
            ISysObjectSettingAppService sysObjectSettingAppService)
        {
            _roleAppService = roleAppService;
            _asysObjectSettingAppService = sysObjectSettingAppService;
        }

        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public async Task<PartialViewResult> PermissionsSelection(string identifier, string[] groupNames)
        {
            ViewBag.GroupNames = groupNames;
            ViewBag.Identifier = identifier;

            var list = await _roleAppService.GetAllPermissions();
            return PartialView("_PermissionsSelection", list);
        }

        [HttpPost]
        public async Task<ActionResult> GetRoles()
        {   //不到这，被重写
            var list = await _roleAppService.GetRoles(null);
            return Json(list);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRoleAsync(CreateRoleDto input)
        {
            await _roleAppService.CreateRoleAsync(input);
            return Json(new { });
        }

        public async Task<ActionResult> Details(int roleId)
        {
            var model = await _roleAppService.GetRoleDto(roleId);
            return View(model);
        }

        protected string GetAccessLevelStr(AccessLevel level)
        {
            return string.Join(", ", 
                level.GetFlags().Select(
                    x => L(x.SouceName())
                  ));
        }

        public async Task<ActionResult> GetSysObjectSettings(int roleId)
        {
            var list = await _asysObjectSettingAppService.GetSysObjectSettingOutputsForRoleAsync(roleId);
            var models = list.Select(x => new
            {
                x.SysObjectName,
                x.AccessLevelInt,
                AccessLevelStr = GetAccessLevelStr(x.AccessLevel)
            }).ToList();
            return Json(models);
        }

        public async Task<ActionResult> GetSysColumnSettingsAsync(int roleId, string sysObjectName)
        {
            var list = await _asysObjectSettingAppService.GetSysColumnSettingOutputsForRoleAsync(roleId, sysObjectName);
            var models = list.Select(x => new
            {
                x.SysObjectName,
                x.SysColumnName,
                x.AccessLevel,
                AccessLevelStr = GetAccessLevelStr(x.AccessLevel)
            }).ToList();
            return Json(models);
        }
    }
}
