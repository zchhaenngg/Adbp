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
                LoginPath = new PathString("/ZeroAccount/Login")
            });
        }
    }
}
