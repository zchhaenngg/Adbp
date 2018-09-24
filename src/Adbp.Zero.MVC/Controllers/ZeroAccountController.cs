using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Adbp.Zero.Authorization;
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Users;
using Adbp.Zero.MultiTenancy;
using Adbp.Zero.MVC.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Adbp.Zero.MVC.Controllers
{
    [AbpMvcAuthorize]
    public class ZeroAccountController : ZeroControllerBase
    {
        private readonly IRepository<LoginAgent, long> _loginAgentRepository;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ITenantCache _tenantCache;
        private readonly UserManager _userManager;
        private readonly LogInManager _logInManager;
        private readonly IAuthenticationManager _authenticationManager;
        
        public ZeroAccountController(
            IRepository<LoginAgent, long> loginAgentRepository,

            IMultiTenancyConfig multiTenancyConfig,
            ITenantCache tenantCache,
            UserManager userManager,
            LogInManager logInManager,
            IAuthenticationManager authenticationManager)
        {
            _loginAgentRepository = loginAgentRepository;
            _multiTenancyConfig = multiTenancyConfig;
            _tenantCache = tenantCache;
            _userManager = userManager;
            _logInManager = logInManager;
            _authenticationManager = authenticationManager;
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        #region Login / Logout

        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            return View(
                new LoginFormViewModel
                {
                    ReturnUrl = returnUrl,
                    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                    IsSelfRegistrationAllowed = IsSelfRegistrationEnabled()
                });
        }

        [AllowAnonymous]
        [HttpPost]
        [DisableAuditing]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            CheckModelState();

            var loginResult = await GetLoginResultAsync(
                loginModel.UsernameOrEmailAddress,
                loginModel.Password,
                GetTenancyNameOrNull()
                );

            await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            return Json(new AjaxResponse { TargetUrl = returnUrl });
        }
        
        public async Task<ActionResult> AgentLogin(long agentId, string returnUrl = "")
        {
            var loginId = AbpSession.GetUserId();
            var userAgent = await _loginAgentRepository.FirstOrDefaultAsync(x => x.PrincipalId == loginId && x.AgentId == agentId);
            await SignInAsync(userAgent.Agent);
            
            if (!string.IsNullOrWhiteSpace(returnUrl) && Request.Url != null && AbpUrlHelper.IsLocalUrl(Request.Url, returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect(Request.ApplicationPath);
        }

        public ActionResult Logout()
        {
            _authenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        #endregion
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginWithFriendlyExceptionAsync(usernameOrEmailAddress, password, tenancyName);
            return loginResult;
        }

        private async Task SignInAsync(User user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            _authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        private bool IsSelfRegistrationEnabled()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return false; //No registration enabled for host users!
            }

            return true;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }
    }
}