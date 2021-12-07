using System.Reflection;

namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static bool IsField<T>(this Expression<Func<T, object>> expression)
            => expression.GetMember() is FieldInfo;

        public static bool IsProperty<T>(this Expression<Func<T, object>> expression)
            => expression.GetMember() is PropertyInfo;

        public static FieldInfo GetField<T>(this Expression<Func<T, object>> expression)
            => expression.GetMember() as FieldInfo;

        public static PropertyInfo GetProperty<T>(this Expression<Func<T, object>> expression)
            => expression.GetMember() as PropertyInfo;

        public static MemberInfo GetMember<T>(this Expression<Func<T, object>> expression)
        {
            LambdaExpression lambda = expression;

            var memberExpression = lambda.Body is UnaryExpression unaryExpression
                ? (MemberExpression)unaryExpression.Operand
                : (MemberExpression)lambda.Body;

            return memberExpression.Member;
        }
    }
}