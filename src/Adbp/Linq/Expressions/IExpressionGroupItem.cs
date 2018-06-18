using System;
using System.Collections.Generic;
using System.Text;

namespace Adbp.Linq.Expressions
{
    public interface IExpressionGroupItem
    {
        bool IsAnd { get; set; }
        string Name { get; set; }
        string Value { get; set; }
        ExpressionOperate Operate { get; set; }
        /// <summary>
        /// whether build expression or not.
        /// </summary>
        IfBuild IfBuild { get; set; }
        bool BeforeBuild();
    }
}
