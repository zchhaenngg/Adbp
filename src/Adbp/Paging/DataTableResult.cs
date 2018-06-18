using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Adbp.Paging
{
    public class DataTableResult<T>
    {
        public DataTableResult(int totalCount, IList<T> rows)
        {
            //recordsTotal = totalCount;
            recordsFiltered = totalCount;
            data = rows;
        }

        public DataTableResult(PagedResultDto<T> pagedResult)
        {

            //如总共5000条记录，查询 姓张的有100条，显示10条
            //即recordsTotal：5000  recordsFiltered：100
            //recordsTotal = pagedResult.TotalCount;
            recordsFiltered = pagedResult.TotalCount;
            data = pagedResult.Items;
        }

        public DataTableResult()
        {
            recordsTotal = 0;
            data = new List<T>();
        }
        //记录了该table发起了多少次查询
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public IEnumerable<T> data { get; set; }
        public string error { get; set; }
    }
}
