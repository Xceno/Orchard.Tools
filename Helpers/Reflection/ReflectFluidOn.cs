namespace Orchard.Tools.Helpers.Reflection {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Orchard.Utility;

    /// <summary>
    /// A helper class to build a list of property names via reflection.
    /// </summary>
    /// <typeparam name="T">The type to reflect upon.</typeparam>
    public class ReflectFluidOn<T> {

        private readonly List<string> memberNames = new List<string>();

        /// <summary>The property names currently added to the list.</summary>
        public List<string> MemberNames {
            get { return this.memberNames; }
        }

        public ReflectFluidOn<T> AddNameOf(Expression<Action<T>> expression) {
            this.memberNames.Add(ReflectOn<T>.NameOf(expression));
            return this;
        }

        public ReflectFluidOn<T> AddNameOf<TResult>(Expression<Func<T, TResult>> expression) {
            this.memberNames.Add(ReflectOn<T>.NameOf(expression));
            return this;
        }

        /// <summary>Removes all elements from the list.</summary>
        public void Clear() {
            this.memberNames.Clear();
        }

        /// <summary>
        /// Returns the names of all public properties of the given type.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetAllPropertyNames() {
            // No BindingFlags necessary. Instance, Public and Static are the defaults.
            return typeof(T).GetProperties().Select(x => x.Name);
        }
    }
}