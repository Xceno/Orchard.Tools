namespace Orchard.Tools.Helpers.Resources {
    using Orchard.UI.Resources;

    /// <summary>Provides extension methods for the <c>Orchard.UI.Resources.ResourceManifest</c></summary>
    public static class ResourceManifestExtensions {
        
        /// <summary>
        /// Adds a stylesheet to the resourceManifest.
        /// The style sheets name will be converted to a minified and a normal resource url.
        /// </summary>
		public static ResourceDefinition DefineStyleWithAllUrls(this ResourceManifest manifest, string name, params string[] subfolders) {
            var subfolderPath = string.Join("/", subfolders);
            return manifest.DefineStyle(name).SetUrl(string.Format("{0}/{1}.min.css", subfolderPath, name), string.Format("{0}/{1}.css", subfolderPath, name));
        }

        /// <summary>
        /// Adds a stylesheet to the resourceManifest.
        /// The style sheets name will be converted to a minified and a normal resource url.
        /// </summary>
        public static ResourceDefinition DefineStyleWithAllUrls(this ResourceManifest manifest, string name) {
            return manifest.DefineStyle(name).SetUrl(string.Format("{0}.min.css", name), string.Format("{0}.css", name));
        }

        /// <summary>
        /// Adds a script to the resourceManifest.
        /// The scripts name will be converted to a minified and a normal resource url.
        /// </summary>
        public static ResourceDefinition DefineScriptWithAllUrls(this ResourceManifest manifest, string name) {
            return manifest.DefineScript(name).SetUrl(string.Format("{0}.min.js", name), string.Format("{0}.js", name));
        }
    }
}