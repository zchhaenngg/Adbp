using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace Adbp.Linq.Expressions
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate, IExpressionGroupItem item)
        {
            return ExpressionBuilder.Create(predicate, item.Name, item.Value, item.Operate, item.IsAnd);
        }
        
        public static Expression<Func<T, bool>> Create<T>(IList<IExpressionGroupItem> items)
        {
            if (items == null)
            {
                return f => true;
            }
            items = items.Where(x => x.BeforeBuild()).ToList();
            if (items.Count == 0)
            {
                return f => true;
            }
            Expression<Func<T, bool>> predicate;
            if (items[0].IsAnd)//利用短路与和短路或的特性实现了忽略本表达式结果。
            {
                predicate = f => true;
            }
            else
            {
                predicate = f => false;
            }
            foreach (var item in items)
            {
                predicate = Create(predicate, item);
            }
            return predicate;
        }

        public static Expression<Func<T, bool>> Create<T>(ExpressionGroup group)
        {
            if (group == null)
            {
                return f => true;
            }
            return Create<T>(group.Items);
        }
    }
}
