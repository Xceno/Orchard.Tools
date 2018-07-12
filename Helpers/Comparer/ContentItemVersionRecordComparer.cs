namespace Orchard.Tools.Helpers {
    using System.Collections.Generic;
    using Orchard.ContentManagement.Records;

    public class ContentItemVersionRecordComparer : IEqualityComparer<ContentItemVersionRecord> {
        public bool Equals(ContentItemVersionRecord x, ContentItemVersionRecord y) {
            return x.Id == y.Id;
        }

        public int GetHashCode(ContentItemVersionRecord obj) {
            return obj.Id.GetHashCode();
        }
    }
}