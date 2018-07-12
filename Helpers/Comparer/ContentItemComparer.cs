namespace Orchard.Tools.Helpers {
    using System.Collections.Generic;
    using Orchard.ContentManagement;

    public class ContentItemComparer<T> : IEqualityComparer<T>
        where T : IContent {
        public bool Equals(T x, T y) {
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj) {
            return obj.Id.GetHashCode();
        }
    }
}