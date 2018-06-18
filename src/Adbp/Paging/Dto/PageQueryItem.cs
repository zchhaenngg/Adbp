using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adbp.Linq.Expressions;

namespace Adbp.Paging.Dto
{
    public class PageQueryItem: ExpressionGroupItem
    {
        public PageQueryItem(
            string name, 
            string value,
            ExpressionOperate operate = ExpressionOperate.Equal,
            IfBuild ifBuild = IfBuild.IsNotNullOrWhiteSpace)
        {
            Name = name;
            Value = value;
            Operate = operate;
            IfBuild = ifBuild;
        }
    }
}
