using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Navigation;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Threading;
using Adbp.Zero.MVC.Models.Layout;
using Adbp.Zero.MVC.Controllers;
using Abp.Domain.Repositories;
using Adbp.Zero.Authorization.Users;
using Adbp.Zero.Users;

namespace Adbp.Zero.MVC.Controllers
{
    public class ZeroLayoutController : ZeroControllerBase
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly ILanguageManager _languageManager;
        private readonly IUserAppService _userAppService;

        public ZeroLayoutController(
            IUserNavigationManager userNavigationManager,
            ILanguageManager languageManager,
            IUserAppService userAppService
            )
        {
            _userNavigationManager = userNavigationManager;
            _languageManager = languageManager;
            _userAppService = userAppService;
        }

        [ChildActionOnly]
        public PartialViewResult SideBarNav(string activeMenu = "")
        {
            var model = new SideBarNavViewModel
            {
                MainMenu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier())),
                ActiveMenuItemName = activeMenu
            };

            return PartialView("_SideBarNav", model);
        }

        [ChildActionOnly]
        public PartialViewResult LanguageSelection()
        {
            var model = new LanguageSelectionViewModel
            {
                CurrentLanguage = _languageManager.CurrentLanguage,
                Languages = _languageManager.GetLanguages()
            };

            return PartialView("_LanguageSelection", model);
        }

        [ChildActionOnly]
        public PartialViewResult AgentSelection()
        {
            var logid = AbpSession.GetUserId();
            var agents = AsyncHelper.RunSync(() => _userAppService.GetAgentsAsync());
            return PartialView("_AgentSelection", agents);
        }
    }
}