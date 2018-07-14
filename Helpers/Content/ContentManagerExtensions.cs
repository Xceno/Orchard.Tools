namespace Orchard.Tools.Helpers.Content {
    using System.Collections.Generic;
    using Orchard.ContentManagement;
    using Orchard.ContentManagement.Records;
    using Orchard.Core.Common.Models;
    using Orchard.Core.Containers.Models;

    public static class ContentManagerExtensions {
        /// <summary>
        /// Queries the database for the given container and returns a list of the container contents with the specified Part.
        /// </summary>
        public static IEnumerable<TPart> GetContainerContentsOfType<TPart, TRecord>(this IContentManager contentManager, int containerId, VersionOptions versionOptions = null)
            where TPart : ContentPart<TRecord>
            where TRecord : ContentPartRecord {
            return contentManager
                .Query<TPart, TRecord>(versionOptions ?? VersionOptions.Published)
                .Join<CommonPartRecord>()
                .Where(x => x.Container != null && x.Container.Id == containerId)
                .Join<ContainablePartRecord>()
                .OrderByDescending(x => x.Position)
                .List();
        }
    }
}