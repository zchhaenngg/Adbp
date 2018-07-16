using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abp.Web.Mvc.Views;
using Adbp.Zero.MVC.Views;

namespace Adbp.Sample.Web.Views
{
    public abstract class SampleWebViewPageBase : SampleWebViewPageBase<dynamic>
    {

    }

    public abstract class SampleWebViewPageBase<TModel> : ZeroWebViewPageBase<TModel>
    {
        protected SampleWebViewPageBase()
        {
            LocalizationSourceName = SampleConsts.LocalizationSourceName;
        }


    }
}