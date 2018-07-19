namespace Orchard.Tools.Helpers.Content {
    using System.Collections.Generic;
    using System.Linq;
    using ContentManagement;
    using ContentPicker.Fields;

    public static class ContentPartExtensions {
        public static int[] GetContentPickerValues(this ContentPart part, string contentPickerName) {
            var contentPicker = (ContentPickerField)part.Fields.FirstOrDefault(f => f.Name == contentPickerName);
            return contentPicker != null ? contentPicker.Ids : new int[0];
        }

        public static T GetItemFromContentPicker<T>(this ContentPart part, string contentPickerName)
            where T : IContent {
            var contentPicker = (ContentPickerField)part.Fields.FirstOrDefault(f => f.Name == contentPickerName);
            if ( contentPicker != null ) {
                var issue = contentPicker.ContentItems.FirstOrDefault();
                if ( issue != null ) {
                    return issue.As<T>();
                }
            }

            return default(T);
        }

        public static IEnumerable<T> GetItemsFromContentPicker<T>(this ContentPart part, string contentPickerName)
            where T : IContent {
            var contentPicker = (ContentPickerField)part.Fields.FirstOrDefault(f => f.Name == contentPickerName);
            if ( contentPicker != null ) {
                var result = contentPicker.ContentItems.Where(i => i.Is<T>()).Select(i => i.As<T>());
                return result;
            }

            return new T[0];
        }
    }
}