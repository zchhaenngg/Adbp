using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Models;
using Adbp.Linq.Expressions;
using Adbp.Paging;
using Adbp.Paging.Dto;
using Adbp.Zero.Notifications;
using Adbp.Zero.Notifications.Dto;

namespace Adbp.Zero.MVC.Controllers
{
    public class ZeroNotificationsController: ZeroControllerBase
    {
        private readonly INotificationAppService _notificationAppService;

        public ZeroNotificationsController(
            INotificationAppService notificationAppService
            )
        {
            _notificationAppService = notificationAppService;
        }

        public ActionResult Index()
        {
            return View();
        }
        
    }
}
