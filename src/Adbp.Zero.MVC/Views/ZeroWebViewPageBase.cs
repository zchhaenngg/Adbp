using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Web.Mvc.Views;

namespace Adbp.Zero.MVC.Views
{
    public abstract class ZeroWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected virtual string Symbol
            => "@";
    }
}
