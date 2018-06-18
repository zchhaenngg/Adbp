using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Adbp.Extensions;
using System.Linq;

namespace Adbp.Linq.Expressions
{
    public static class ExpressionBuilder
    {
        public static object ChangeType(Type type, string value)
        {
            var tmpType = type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
            var tmpValue = value == null ? null :
                type.IsEnum ? Enum.Parse(tmpType, value) : Convert.ChangeType(value, tmpType);
            return tmpValue;
        }
        public static MemberExpression CreateMember(ParameterExpression exp1, string name)
        {
            return Expression.Property(exp1, name);
        }

        public static ConstantExpression CreateConstant(Type type, string value)
        {
            var tmpValue = ChangeType(type, value);
            return Expression.Constant(tmpValue, type);
        }

        public static Expression CreateEqual(ParameterExpression exp1, string name, Type type, string value)
        {
            var left = CreateMember(exp1, name);
            var right = CreateConstant(type, value);
            return Expression.Equal(left, right);
        }

        public static Expression CreateLike(ParameterExpression exp1, string name, string value)
        {
            var instance = CreateMember(exp1, name);
            var methodInfo = typeof(string).GetMethod(nameof(string.Contains));
            var arguments = CreateConstant(typeof(string), value);
            return Expression.Call(instance, methodInfo, arguments);
        }

        public static Expression CreateContains(ParameterExpression exp1, string name, Type type, string value)
        {
            var strArr = value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            foreach (var item in strArr)
            {
                var method = list.GetType().GetMethod("Add");
                method.Invoke(list, new object[] { ChangeType(type, item) });
            }

            var instance = Expression.Constant(list, list.GetType());//和Like方法不一样，instance是list
            var arguments = CreateMember(exp1, name);
            var methodInfo = list.GetType().GetMethod("Contains");
            return Expression.Call(instance, methodInfo, arguments);
        }

        public static Expression CreateNull(ParameterExpression exp1, string name, Type type)
        {
            var left = CreateMember(exp1, name);
            return Expression.Equal(left, Expression.Constant(null));
        }

        public static Expression CreateNotNull(ParameterExpression exp1, string name, Type type)
        {
            var left = CreateMember(exp1, name);
            return Expression.NotEqual(left, Expression.Constant(null));
        }

        public static Expression Create(ParameterExpression exp1, string name, Type type, string value, ExpressionOperate operate)
        {
            switch (operate)
            {
                case ExpressionOperate.Equal:
                    return CreateEqual(exp1, name, type, value);
                case ExpressionOperate.Like:
                    if (!type.Equals(typeof(string)))
                    {
                        throw new ArgumentException("operate like 's type should be string!");
                    }
                    return CreateLike(exp1, name, value);
                case ExpressionOperate.Contains:
                    return CreateContains(exp1, name, type, value);
                case ExpressionOperate.IsNull:
                    return CreateNull(exp1, name, type);
                case ExpressionOperate.IsNotNull:
                    return CreateNotNull(exp1, name, type);
                default:
                    throw new NotSupportedException(operate.ToString());
            }
        }
        
        public static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> predicate, Expression right, bool isAnd)
        {
            BinaryExpression body = isAnd ?
                   Expression.AndAlso(predicate.Body, right) :
                   Expression.OrElse(predicate.Body, right);
            return Expression.Lambda<Func<T, bool>>(body, predicate.Parameters[0]);
        }

        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate, string name, string value, ExpressionOperate operate, bool isAnd)
        {
            var right = Create(predicate.Parameters[0], name, typeof(T).GetProperty(name).PropertyType, value, operate);
            return Combine(predicate, right, isAnd);
        }
    }
}
