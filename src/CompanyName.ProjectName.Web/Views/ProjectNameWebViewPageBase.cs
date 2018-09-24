using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abp.Web.Mvc.Views;
using Adbp.Zero.MVC.Views;

namespace CompanyName.ProjectName.Web.Views
{
    public abstract class ProjectNameWebViewPageBase : ProjectNameWebViewPageBase<dynamic>
    {

    }

    public abstract class ProjectNameWebViewPageBase<TModel> : ZeroWebViewPageBase<TModel>
    {
        protected ProjectNameWebViewPageBase()
        {
            LocalizationSourceName = ProjectNameConsts.LocalizationSourceName;
        }

        protected HtmlString GetDisabledStrIfNotGranted(string permissionName)
        {
            var disabledStr = IsGranted(permissionName) ? string.Empty :
                "disabled='disabled'";

            return new HtmlString(disabledStr);
        }
    }
}