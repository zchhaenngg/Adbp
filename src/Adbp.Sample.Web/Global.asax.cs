using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Adbp.Sample.Web.App_Start;
using Castle.Facilities.Logging;

namespace Adbp.Sample.Web
{
    public class MvcApplication : AbpWebApplication<SampleWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig(Server.MapPath("log4net.config"))
            );
            base.Application_Start(sender, e);
        }
    }
}
