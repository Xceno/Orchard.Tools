namespace Orchard.Tools.Helpers.Resources {
    using System;
    using Orchard.UI.Resources;

    public static class ResourceDefinitionExtensions {

        /// <summary>
        /// Adds the given url to the ResourceDefinition and appends a cacheBuster to it.
        /// </summary>
        /// <param name="self">The ResourceDefinition file to add the url to.</param>
        /// <param name="url">The URL to add to the ResourceDefinition.</param>
        /// <param name="debugUrl">The url to use in debug mode. Use this to specify an unminified, unobfuscated version of your resource.</param>
        /// <param name="frequent">If true, the cacheBuster will be updated each minute. Otherwise it'll be changed daily.</param>
        public static ResourceDefinition SetUrlWithCacheBuster(this ResourceDefinition self, string url, string debugUrl = null, bool frequent = false) {
            if ( string.IsNullOrWhiteSpace(url) ) {
                throw new ArgumentNullException("url");
            }

            var cacheBustQueryString = frequent
                ? (long)new TimeSpan(DateTime.Now.Ticks - DateTime.MinValue.Ticks).TotalMinutes
                : DateTime.Today.Ticks;

            var urlWithCacheBuster = string.Format("{0}?{1}", url, cacheBustQueryString);

            return !string.IsNullOrWhiteSpace(debugUrl)
                ? self.SetUrl(urlWithCacheBuster, string.Format("{0}?{1}", debugUrl, cacheBustQueryString))
                : self.SetUrl(urlWithCacheBuster);
        }
    }
}