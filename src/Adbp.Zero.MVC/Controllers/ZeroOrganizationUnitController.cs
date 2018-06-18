using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Adbp.Linq.Expressions;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Adbp.Zero.Authorization;
using Adbp.Zero.OrganizationUnits;
using Adbp.Zero.OrganizationUnits.Dto;
using Adbp.Paging.Dto;
using Adbp.Paging;

namespace Adbp.Zero.MVC.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Permissions_OrganizationUnit)]
    public class ZeroOrganizationUnitController : ZeroControllerBase
    {
        private readonly IOrganizationUnitAppService _organizationUnitAppService;

        public ZeroOrganizationUnitController(IOrganizationUnitAppService organizationUnitAppService)
        {
            _organizationUnitAppService = organizationUnitAppService;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [DontWrapResult]
        public async Task<JsonResult> JsonGetOrganizationUserOuputs(int draw, DataTableQuery query, long organizationUnitId)
        {
            IEnumerable<PageQueryItem> getPageQueryItems()
            {
                yield return new PageQueryItem(nameof(OrganizationUnitUserDto.UserName), query.Search.Value, ExpressionOperate.Like);
            }

            var page = await _organizationUnitAppService.GetOrganizationUnitUserPageAsync(
                       new GenericPagingInput(query.Start, query.Length, list: getPageQueryItems().ToList())
                       {
                           Sorting = "CreationTime desc"
                       },
                       organizationUnitId);
            return Json(new DataTableResult<OrganizationUnitUserDto>(page));
        }

        [DontWrapResult]
        public async Task<JsonResult> JsonGetUsersNotInOrganization(int draw, DataTableQuery query, long organizationUnitId, string usernameLike)
        {
            IEnumerable<PageQueryItem> getPageQueryItems()
            {
                yield return new PageQueryItem(nameof(Adbp.Zero.Authorization.Users.User.UserName), query.Search.Value, ExpressionOperate.Like);
            }

            var input = new GenericPagingInput(query.Start, query.Length, list: getPageQueryItems().ToList());
            var page = await _organizationUnitAppService.GetUsersNotInOrganizationAsync(input);
            return Json(new DataTableResult<OrganizationUserOuput>(page));
        }
    }
}