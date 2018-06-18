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
using Adbp.Zero.Authorization.Roles;
using Adbp.Paging;
using Adbp.Paging.Dto;
using Adbp.Zero.Users;
using Adbp.Zero.Users.Dto;

namespace Adbp.Zero.MVC.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Permissions_User)]
    public class ZeroUsersController : ZeroControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IRoleAppService _roleAppService;

        public ZeroUsersController(
            IUserAppService userAppService,
            IRoleAppService roleAppService
            )
        {
            _userAppService = userAppService;
            _roleAppService = roleAppService;
        }

        // GET: User
        public async Task<ActionResult> Index()
        {
            ViewBag.Roles = (await _roleAppService.GetRoles(null)).Items;
            return View();
        }

        [DontWrapResult]
        public async Task<ActionResult> GetUsers(DataTableQuery query)
        {
            IEnumerable<PageQueryItem> getPageQueryItems()
            {
                yield return new PageQueryItem(nameof(Adbp.Zero.Authorization.Users.User.Name), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Adbp.Zero.Authorization.Users.User.Surname), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Adbp.Zero.Authorization.Users.User.UserName), query.Search.Value, ExpressionOperate.Like);
            }

            var input = new GenericPagingInput(
                query.Start,
                query.Length,
                list: getPageQueryItems().ToList());
            //input.Sorting = "CreationTime asc";
            var page = await _userAppService.GetUsers(input);
            return Json(new DataTableResult<UserDto>(page));
        }
    }
}
