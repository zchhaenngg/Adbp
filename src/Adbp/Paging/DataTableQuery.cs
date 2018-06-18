using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Paging
{
    public class DataTableSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }
    public class DataTableColumnSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }
    public class DataTableColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DataTableColumnSearch Search { get; set; }
    }
    public class DataTableColumnOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
        public bool IsDesc => string.Equals(Dir, "desc", StringComparison.OrdinalIgnoreCase);
    }
    /// <summary>
    /// https://datatables.net
    /// </summary>
    public class DataTableQuery
    {
        /// <summary>
        /// linq to SQL, Skip
        /// </summary>
        public virtual int Start { get; set; }
        /// <summary>
        /// linq to SQL, Take
        /// </summary>
        public virtual int Length { get; set; }

        public DataTableColumn[] Columns { get; set; }

        public DataTableColumnOrder[] Order { get; set; }

        public DataTableSearch Search { get; set; }
    }
}
