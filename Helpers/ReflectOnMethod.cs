namespace Orchard.Tools.Helpers {
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Ripped from here: http://ideone.com/YVsSOO
    /// Based on this answer: https://stackoverflow.com/a/26976055/932315
    /// </summary>
    public static class ReflectOnMethod<T> {
        public static string NameOf(Expression<Func<T, Action>> expression) {
            return MethodName(expression);
        }

        public static string NameOf<TParam>(Expression<Func<T, Action<TParam>>> expression) {
            return MethodName(expression);
        }

        public static string NameOf<TResult>(Expression<Func<T, Func<TResult>>> expression) {
            return MethodName(expression);
        }

        public static string NameOf<TParam, TResult>(Expression<Func<T, Func<TParam, TResult>>> expression) {
            return MethodName(expression);
        }

        public static string NameOf<TParam1, TParam2, TResult>(Expression<Func<T, Func<TParam1, TParam2, TResult>>> expression) {
            return MethodName(expression);
        }

        public static string NameOf<TParam1, TParam2, TParam3, TResult>(Expression<Func<T, Func<TParam1, TParam2, TParam3, TResult>>> expression) {
            return MethodName(expression);
        }

        public static string NameOf<TParam1, TParam2, TParam3, TParam4, TResult>(Expression<Func<T, Func<TParam1, TParam2, TParam3, TParam4, TResult>>> expression) {
            return MethodName(expression);
        }

        public static string NameOf<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(Expression<Func<T, Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>>> expression) {
            return MethodName(expression);
        }

        public static string MethodName(LambdaExpression expression) {
            var unaryExpression = (UnaryExpression)expression.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;

            var methodCallObject = (ConstantExpression)methodCallExpression.Object;
            var methodInfo = (MethodInfo)methodCallObject.Value;
            return methodInfo.Name;
        }
    }
}