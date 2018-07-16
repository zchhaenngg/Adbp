using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Adbp.Linq.Expressions;
using Abp.Web.Models;
using Adbp.Zero.Analasy;
using Adbp.Zero.Analasy.Dto;
using Adbp.Paging;
using Adbp.Paging.Dto;
using Abp.Domain.Repositories;
using Abp.Web.Mvc.Authorization;
using Adbp.Zero.Authorization;

namespace Adbp.Zero.MVC.Controllers
{
    [AbpMvcAuthorize]
    public class ZeroAnalasyController : ZeroControllerBase
    {
        private readonly IRepository<Adbp.Zero.Authorization.Users.User, long> _userRepository;
        private readonly IAnalasyAppService _analasyAppService;

        public ZeroAnalasyController(
            IRepository<Adbp.Zero.Authorization.Users.User, long> userRepository,
            IAnalasyAppService analasyAppService)
        {
            _userRepository = userRepository;
            _analasyAppService = analasyAppService;
        }

        [AbpMvcAuthorize(ZeroPermissionNames.Permissions_AuditLog)]
        public ActionResult AuditLog()
        {
            return View();
        }

        [AbpMvcAuthorize(ZeroPermissionNames.Permissions_LoginAttemptLog)]
        public ActionResult LoginAttemptLog()
        {
            return View();
        }

        [AbpMvcAuthorize(ZeroPermissionNames.Permissions_AuditLog)]
        [DontWrapResult]
        public async Task<ActionResult> GetAuditLogs(DataTableQuery query)
        {
            IEnumerable<PageQueryItem> getPageQueryItems()
            {//AuditLog
                if (query.Search.Value?.Trim() == ZeroConsts.SearchCmds.SymbolException)
                {
                    yield return new PageQueryItem(nameof(Abp.Auditing.AuditLog.Exception), query.Search.Value, ExpressionOperate.IsNotNull);
                    yield break;
                }
                yield return new PageQueryItem(nameof(Abp.Auditing.AuditLog.BrowserInfo), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Abp.Auditing.AuditLog.ClientIpAddress), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Abp.Auditing.AuditLog.ClientName), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Abp.Auditing.AuditLog.Exception), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Abp.Auditing.AuditLog.MethodName), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Abp.Auditing.AuditLog.Parameters), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Abp.Auditing.AuditLog.ServiceName), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Abp.Auditing.AuditLog.UserId), 
                    UserLikeStr(_userRepository, query.Search.Value), ExpressionOperate.Contains);
            }
            var input = new GenericPagingInput(
                query.Start,
                query.Length,
                list: getPageQueryItems().ToList());
            var page = await _analasyAppService.GetAuditLogsAsync(input);
            return Json(new DataTableResult<AuditLogDto>(page));
        }

        [AbpMvcAuthorize(ZeroPermissionNames.Permissions_LoginAttemptLog)]
        [DontWrapResult]
        public async Task<ActionResult> GetUserLoginAttempts(DataTableQuery query)
        {
            IEnumerable<PageQueryItem> getPageQueryItems()
            {//UserLoginAttempt
                yield return new PageQueryItem(nameof(UserLoginAttempt.BrowserInfo), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(UserLoginAttempt.ClientName), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(UserLoginAttempt.ClientIpAddress), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(UserLoginAttempt.UserNameOrEmailAddress), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(UserLoginAttempt.Result),
                        EnumLikeStr<AbpLoginResultType>(query.Search.Value), ExpressionOperate.Contains);
            }
            var input = new GenericPagingInput(
                query.Start,
                query.Length,
                list: getPageQueryItems().ToList());
            var page = await _analasyAppService.GetUserLoginAttemptsAsync(input);
            return Json(new DataTableResult<UserLoginAttemptDto>(page));
        }

        public ActionResult SelfLogins()
        {
            return View();
        }
        
        [DontWrapResult]
        public async Task<ActionResult> GetLoginAttempts(DataTableQuery query)
        {
            IEnumerable<PageQueryItem> getPageQueryItems()
            {//UserLoginAttempt
                yield return new PageQueryItem(nameof(UserLoginAttempt.BrowserInfo), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(UserLoginAttempt.ClientName), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(UserLoginAttempt.ClientIpAddress), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(UserLoginAttempt.UserNameOrEmailAddress), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(UserLoginAttempt.Result),
                        EnumLikeStr<AbpLoginResultType>(query.Search.Value), ExpressionOperate.Contains);
            }
            var input = new GenericPagingInput(
                query.Start,
                query.Length,
                list: getPageQueryItems().ToList());
            var page = await _analasyAppService.GetSelfLoginAttemptsAsync(input);
            return Json(new DataTableResult<UserLoginAttemptDto>(page));
        }
    }
}