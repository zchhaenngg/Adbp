using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Paging.Dto
{
    public interface IPageQueryItems
    {
        IList<PageQueryItem> QueryItems { get; set; }
    }
}
