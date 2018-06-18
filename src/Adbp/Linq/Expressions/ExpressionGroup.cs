using System;
using System.Collections.Generic;
using System.Text;

namespace Adbp.Linq.Expressions
{
    /// <summary>
    /// 1 Group -> 1 Expression, 1 Expression 共用1个ParameterExpression
    /// </summary>
    public class ExpressionGroup
    {//类似于 Folder
        //暂时不支持
        //public IList<ExpressionGroup> Groups { get; set; }
        public IList<IExpressionGroupItem> Items { get; set; }
    }
}
