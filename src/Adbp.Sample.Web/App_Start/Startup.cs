using System;
using System.Threading.Tasks;
using Adbp.Sample.Web.App_Start;
using Microsoft.Owin;
using Owin;
using Abp.Owin;
using Adbp.Sample.Web.Controllers;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Adbp.Zero.MVC.Controllers;
using Adbp.Sample.Api.Controllers;
using System.Configuration;

[assembly: OwinStartup(typeof(Startup))]

namespace Adbp.Sample.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAbp();

            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/ZeroAccount/Login"),
                // by setting following values, the auth cookie will expire after the configured amount of time (default 14 days) when user set the (IsPermanent == true) on the login
                ExpireTimeSpan = new TimeSpan(int.Parse(ConfigurationManager.AppSettings["AuthSession.ExpireTimeInDays.WhenPersistent"] ?? "14"), 0, 0, 0),
                SlidingExpiration = bool.Parse(ConfigurationManager.AppSettings["AuthSession.SlidingExpirationEnabled"] ?? bool.FalseString)
            });
        }
    }
}
