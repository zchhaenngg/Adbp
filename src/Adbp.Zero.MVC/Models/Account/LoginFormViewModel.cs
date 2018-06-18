using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adbp.Zero.MVC.Models.Account
{
    public class LoginFormViewModel
    {
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 是否支持多租户
        /// </summary>
        public bool IsMultiTenancyEnabled { get; set; }

        /// <summary>
        /// 是否支持用户可以自己注册
        /// </summary>
        public bool IsSelfRegistrationAllowed { get; set; }
    }
}