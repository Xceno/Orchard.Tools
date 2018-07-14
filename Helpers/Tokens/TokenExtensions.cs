namespace Orchard.Tools.Helpers.Tokens {
    using System;
    using System.Linq.Expressions;
    using Orchard.Localization;
    using Orchard.Tokens;
    using Orchard.Tools.Helpers.Reflection;

    public static class TokenExtensions {
        public static DescribeFor TokenFor<T>(this DescribeFor self, Expression<Func<T, object>> expression, LocalizedString name, LocalizedString description, string chainTarget = null) {
#pragma warning disable CS0618 // We need enum support here
            var property = ObjectExtensions.PropertyName(expression);
#pragma warning restore CS0618 // Type or member is obsolete
            return self.Token(property, name, description, chainTarget);
        }
    }
}