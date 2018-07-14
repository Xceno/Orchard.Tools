namespace Orchard.Tools.Helpers.Reflection {
    using System;
    using System.Linq.Expressions;

    [Obsolete("Please use orchards ReflectOn<T> extension methods unless you need to handle an enum. https://github.com/OrchardCMS/Orchard/issues/7411")]
    public static class ObjectExtensions {
        public static string PropertyName<T>(Expression<Func<T, object>> expression) {
            var body = expression.Body as MemberExpression ?? ((UnaryExpression)expression.Body).Operand as MemberExpression;
            return body.Member.Name;
        }

        public static string PropertyName<T>(this T self, Expression<Func<T, object>> expression) {
            return PropertyName(expression);
        }
    }
}