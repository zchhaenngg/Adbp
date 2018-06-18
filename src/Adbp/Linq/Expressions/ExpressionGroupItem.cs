using System;
using System.Collections.Generic;
using System.Text;

namespace Adbp.Linq.Expressions
{
    public class ExpressionGroupItem: IExpressionGroupItem
    {//类似于File
        public bool IsAnd { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public ExpressionOperate Operate { get; set; }
        /// <summary>
        /// whether build expression or not.
        /// </summary>
        public IfBuild IfBuild { get; set; }

        /// <summary>
        /// if true build expression otherwise ignore.
        /// </summary>
        /// <returns></returns>
        public bool BeforeBuild()
        {
            switch (IfBuild)
            {
                case IfBuild.IsNotNullOrWhiteSpace:
                    return !string.IsNullOrWhiteSpace(Value);
                case IfBuild.IsNullOrWhiteSpace:
                    return string.IsNullOrWhiteSpace(Value);
                default:
                    throw new NotSupportedException(IfBuild.ToString());
            }
        }
    }

    public enum ExpressionOperate
    {
        Equal = 1,
        /// <summary>
        /// 字符串模糊匹配
        /// </summary>
        Like = 2,
        /// <summary>
        /// 对容器的操作，非字符串
        /// </summary>
        Contains = 3,
        /// <summary>
        /// 为空
        /// </summary>
        IsNull = 4,
        /// <summary>
        /// 不为空
        /// </summary>
        IsNotNull =5
    }

    public enum IfBuild
    {
        IsNotNullOrWhiteSpace = 0,
        IsNullOrWhiteSpace = 1
    }
}
