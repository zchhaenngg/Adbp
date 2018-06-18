using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Adbp.Paging.Dto
{
    public class GenericPagingInput : IPagedAndSortedResultRequest, IPageQueryItems
    {
        public GenericPagingInput(int skip = 0, int pageSize = 10, List<PageQueryItem> list = null)
        {
            SkipCount = skip;
            MaxResultCount = pageSize;
            QueryItems = list;
        }

        public int SkipCount { get; set; }
        /// <summary>
        /// page size
        /// </summary>
        public int MaxResultCount { get; set; } = 10;
        public string Sorting { get; set; } = "id desc";
        public IList<PageQueryItem> QueryItems { get; set; }
    }
}
