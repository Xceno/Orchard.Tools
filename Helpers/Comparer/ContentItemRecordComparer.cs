namespace Orchard.Tools.Helpers.Comparer {
    using System.Collections.Generic;
    using Orchard.ContentManagement.Records;

    public class ContentItemRecordComparer : IEqualityComparer<ContentItemRecord> {
        public bool Equals(ContentItemRecord x, ContentItemRecord y) {
            return x != null && y != null && x.Id == y.Id;
        }

        public int GetHashCode(ContentItemRecord obj) {
            return obj.Id.GetHashCode();
        }
    }
}