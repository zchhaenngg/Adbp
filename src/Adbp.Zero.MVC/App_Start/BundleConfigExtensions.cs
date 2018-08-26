using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Adbp.Zero.MVC
{
    public static class BundleConfigExtensions
    {
        public static Bundle IncludeJs(this Bundle bundle, IEnumerable<string> virtualPaths)
        {
            return bundle.Include(virtualPaths.ToArray());
        }

        public static Bundle IncludeCss(this Bundle bundle, IEnumerable<string> virtualPaths)
        {
            foreach (var item in virtualPaths)
            {
                bundle = bundle.Include(item, new CssRewriteUrlTransform());
            }
            return bundle;
        }
    }
}
