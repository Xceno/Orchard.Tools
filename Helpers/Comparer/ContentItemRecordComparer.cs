namespace Orchard.Tools.Helpers {
    using System.Collections.Generic;
    using Orchard.ContentManagement.Records;

    public class ContentItemRecordComparer : IEqualityComparer<ContentItemRecord> {
        public bool Equals(ContentItemRecord x, ContentItemRecord y) {
            return x.Id == y.Id;
        }

        public int GetHashCode(ContentItemRecord obj) {
            return obj.Id.GetHashCode();
        }
    }
}