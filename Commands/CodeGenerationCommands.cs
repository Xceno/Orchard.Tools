namespace Orchard.Tools.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Orchard;
    using Orchard.Commands;
    using Orchard.Environment.Extensions;

    /// <summary>
    /// Original code by Piotr Szmyd.
    /// http://gallery.orchardproject.net/Packages/Orchard.Module.Szmyd.CodeGeneration
    /// </summary>
    public class CodeGenerationCommands : DefaultOrchardCommandHandler
    {
        private static readonly IDictionary<string, TemplateInfo> filesInfo =
            new Dictionary<string, TemplateInfo>
            {
                { "PartHandler", new TemplateInfo { Name = "$$PartName$$Handler.cs", Dir = "Handlers/", IsCompiled = true } },
                { "PartDriver", new TemplateInfo { Name = "$$PartName$$Driver.cs", Dir = "Drivers/", IsCompiled = true } },
                { "PartModel", new TemplateInfo { Name = "$$PartName$$.cs", Dir = "Models/", IsCompiled = true } },
                { "PartRecord", new TemplateInfo { Name = "$$CleanPartName$$Record.cs", Dir = "Models/", IsCompiled = true } },
                { "PartDisplayShape", new TemplateInfo { Name = "$$PartShapeFileName$$.cshtml", Dir = "Views/Parts/", IsCompiled = false } },
                { "PartEditorShape", new TemplateInfo { Name = "$$PartShapeFileName$$.cshtml", Dir = "Views/EditorTemplates/Parts/", IsCompiled = false } }
            };

        private static readonly IDictionary<string, TemplateInfo> settingsFilesInfo =
            new Dictionary<string, TemplateInfo>
            {
                { "TypePartSettings", new TemplateInfo { Name = "$$PartName$$TypePartSettings.cs", Dir = "Settings/", IsCompiled = true } },
                { "TypePartEditorShape", new TemplateInfo { Name = "$$PartName$$TypePartSettings.cshtml", Dir = "Views/DefinitionTemplates/", IsCompiled = false } }
            };

        private static readonly string templatePath = HostingEnvironment.MapPath("~/Modules/Orchard.Tools/Templates/");
        private readonly IExtensionManager extensionManager;

        public CodeGenerationCommands(IExtensionManager extensionManager)
        {
            this.extensionManager = extensionManager;
        }

        [OrchardSwitch]
        public string Properties { get; set; }

        [OrchardSwitch]
        public string ForFeature { get; set; }

        [OrchardSwitch]
        public string ShapeFile { get; set; }

        [OrchardSwitch]
        public string AttachTo { get; set; }

        [OrchardSwitch]
        public bool IsTheme { get; set; }

        [CommandHelp("codegen part <module-name> <part-name> [/ShapeFile:<shape-filename>] [/ForFeature:<feature-name>] [/AttachTo:<content-type>] [/Properties:<list>]\r\n\t"
                     + "Creates a content part inside a given Orchard module.\r\n\t"
                     + "Optional parameters:\r\n\t"
                     + "/Properties - comma-delimited list of name:type values, eg. Name:string,Count:int\r\n\t"
                     + "/ShapeFile - name of your part's shape .cshtml file (if not specified, the name will be used)\r\n\t"
                     + "/AttachTo - specify the content type to attach your part to (hardcoded in the handler via ActivatingFilter)\r\n\t"
                     + "/ForFeature - specify the feature to which your part has to be assigned to (if any)")]
        [CommandName("codegen part")]
        [OrchardSwitches("Properties, ShapeFile, AttachTo, ForFeature")]
        public void CreatePart(string moduleName, string partName)
        {
            this.Context.Output.WriteLine(this.T("Creating content part {0} in module {1}", partName, moduleName));

            var extensionDescriptor = this.extensionManager.GetExtension(moduleName);

            if ( extensionDescriptor == null )
            {
                throw new OrchardException(this.T("Creating content part {0} failed: target module {1} could not be found.", partName, moduleName));
            }

            var partNameWithoutSuffix = partName;
            if ( partName.EndsWith("Part") )
            {
                partNameWithoutSuffix = partName.Substring(0, partName.LastIndexOf("Part", StringComparison.Ordinal));
            }

            var shapeName = ( string.IsNullOrWhiteSpace(this.ShapeFile) ? partNameWithoutSuffix : this.ShapeFile )
                .Trim().Replace('.', '_').Replace("-", "__");
            var shapeFileName = string.IsNullOrWhiteSpace(this.ShapeFile) ? partNameWithoutSuffix : this.ShapeFile;
            var featureAttribute = string.IsNullOrWhiteSpace(this.ForFeature) ? string.Empty : string.Format("[OrchardFeature(\"{0}\")]", this.ForFeature);
            var partActivatingFilter = string.IsNullOrWhiteSpace(this.AttachTo) ? string.Empty : string.Format("Filters.Add(new ActivatingFilter<{0}>(\"{1}\"));", partName, this.AttachTo);

            var replacements = new Dictionary<string, string>
            {
                { "PartName", partName },
                { "CleanPartName", partNameWithoutSuffix },
                { "ModuleName", moduleName },
                { "PartShapeFileName", shapeFileName },
                { "PartShapeName", "Parts_" + shapeName },
                { "PartFeatureAttribute", featureAttribute },
                { "PartActivatingFilter", partActivatingFilter }
            };

            var extensionFolder = this.IsTheme ? "~/Themes" : "~/Modules";

            var csProjPath = HostingEnvironment.MapPath(string.Format("{1}/{0}/{0}.csproj", extensionDescriptor.Id, extensionFolder));

            // Filling properties according to provided data
            try
            {
                var properties = string.IsNullOrWhiteSpace(this.Properties)
                    ? new KeyValuePair<string, string>[0]
                    : this.Properties.Trim()
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries))
                        .Select(s => new KeyValuePair<string, string>(s[0].Trim(), s[1].Trim()))
                        .ToArray();

                var modelProps = properties.Aggregate(string.Empty, (current, s) =>
                    current + FillTemplate("PartModelProperty", new Dictionary<string, string> { { "PropertyName", s.Key }, { "PropertyType", s.Value } }));
                var recordProps = properties.Aggregate(string.Empty, (current, s) =>
                    current + FillTemplate("PartRecordProperty", new Dictionary<string, string> { { "PropertyName", s.Key }, { "PropertyType", s.Value } }));
                var editorShapeFields = properties.Aggregate(string.Empty, (current, s) =>
                    current + FillTemplate("PartEditorShapeFormField", new Dictionary<string, string> { { "PropertyName", s.Key }, { "PropertyType", s.Value } }));

                replacements.Add("PartModelProperties", modelProps);
                replacements.Add("PartRecordProperties", recordProps);
                replacements.Add("PartEditorShapeFormFields", editorShapeFields);
            }
            catch ( Exception )
            {
                throw new OrchardException(this.T("Creating content part {0} failed: Incorrect properties definition.", partName, moduleName));
            }

            // Create dirs
            foreach ( var kv in filesInfo )
            {
                // Prepare templated file names
                kv.Value.Name = replacements.Aggregate(kv.Value.Name,
                    (current, s) => current.Replace(string.Format("$${0}$$", s.Key), s.Value));

                var dirPath = HostingEnvironment.MapPath(extensionFolder + "/" + extensionDescriptor.Id + "/" + kv.Value.Dir);
                var filePath = HostingEnvironment.MapPath(extensionFolder + "/" + extensionDescriptor.Id + "/" + kv.Value.Dir + kv.Value.Name);
                if ( !Directory.Exists(dirPath) )
                {
                    Directory.CreateDirectory(dirPath);
                }
                if ( File.Exists(filePath) )
                {
                    throw new OrchardException(this.T("{2} for {0} already exists in target module {1}.", partName, moduleName, kv.Key));
                }
            }

            // Generating files from templates
            foreach ( var kv in filesInfo )
            {
                var outputPath = HostingEnvironment.MapPath(extensionFolder + "/" + extensionDescriptor.Id + "/" + kv.Value.Dir + kv.Value.Name);
                FillTemplateAndCreateFile(kv.Key, outputPath, replacements);
                this.AddToModuleProject(kv.Value.Dir + kv.Value.Name, csProjPath, kv.Value.IsCompiled);
                this.Context.Output.WriteLine(this.T("{1} for {0} created successfully", partName, kv.Key));
            }

            // Refreshing placement.info file
            var placementPath = HostingEnvironment.MapPath(extensionFolder + "/" + extensionDescriptor.Id + "/Placement.info");
            var placementText = placementPath != null && File.Exists(placementPath) ? File.ReadAllText(placementPath) : File.ReadAllText(templatePath + "Placement.txt");
            var itemGroupReference = string.Format("    <Place {0}=\"Content:before\"/>\r\n", replacements["PartShapeName"]);
            placementText = placementText.Insert(placementText.LastIndexOf("</Placement>", StringComparison.Ordinal), itemGroupReference);

            if ( placementPath != null )
            {
                if ( !File.Exists(placementPath) )
                {
                    this.AddToModuleProject("Placement.info", csProjPath, false);
                }
                File.WriteAllText(placementPath, placementText);
            }

            this.Context.Output.WriteLine(this.T("Content part {0} created successfully in module {1}", partName, moduleName));
        }

        [CommandHelp("codegen typesettings <module-name> <part-name> [/Properties:<list>]\r\n\t"
                     + "Optional parameters:\r\n\t"
                     + "/Properties - comma-delimited list of name:type values, eg. Name:string,Count:int\r\n\t")]
        [CommandName("codegen typesettings")]
        [OrchardSwitches("Properties")]
        public void CreateSettings(string moduleName, string partName)
        {
            this.Context.Output.WriteLine(this.T("Creating content part {0} type settings in module {1}", partName, moduleName));

            var extensionDescriptor = this.extensionManager.GetExtension(moduleName);

            if ( extensionDescriptor == null )
            {
                throw new OrchardException(this.T("Creating content part {0} type settings failed: target module {1} could not be found.", partName, moduleName));
            }

            var replacements = new Dictionary<string, string>
                               {
                                   { "PartName", partName },
                                   { "ModuleName", moduleName },
                               };

            var extensionFolder = "~/Modules";

            var csProjPath = HostingEnvironment.MapPath(string.Format("{1}/{0}/{0}.csproj", extensionDescriptor.Id, extensionFolder));

            // Filling properties according to provided data
            try
            {
                var properties = string.IsNullOrWhiteSpace(this.Properties)
                    ? Enumerable.Empty<KeyValuePair<string, string>>()
                    : this.Properties.Trim()
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries))
                        .Select(s => new KeyValuePair<string, string>(s[0].Trim(), s[1].Trim()));

                var settingsModelProps = properties.Aggregate(string.Empty, (current, s) =>
                    current + FillTemplate("TypePartProperty", new Dictionary<string, string> { { "PropertyName", s.Key }, { "PropertyType", s.Value } }));

                var settingsEditorFields = properties.Aggregate(string.Empty, (current, s) =>
                    current + FillTemplate("TypePartEditorShapeFormField", new Dictionary<string, string> { { "PropertyName", s.Key }, { "PropertyType", s.Value } }));

                var builderLines = properties.Aggregate(string.Empty, (current, s) =>
                    current + FillTemplate("TypePartBuilderSetting", new Dictionary<string, string> { { "PropertyName", s.Key }, { "PropertyType", s.Value }, { "PartName", partName } }));

                replacements.Add("TypePartProperties", settingsModelProps);
                replacements.Add("TypePartEditorShapeFormFields", settingsEditorFields);
                replacements.Add("TypePartBuilderSettings", builderLines);
            }
            catch ( Exception )
            {
                throw new OrchardException(this.T("Creating content part {0} type settings failed: Incorrect properties definition.", partName, moduleName));
            }

            // Create dirs
            foreach ( var kv in settingsFilesInfo )
            {
                // Prepare templated file names
                kv.Value.Name = replacements.Aggregate(kv.Value.Name,
                    (current, s) => current.Replace(string.Format("$${0}$$", s.Key), s.Value));

                var dirPath = HostingEnvironment.MapPath(extensionFolder + "/" + extensionDescriptor.Id + "/" + kv.Value.Dir);
                var filePath = HostingEnvironment.MapPath(extensionFolder + "/" + extensionDescriptor.Id + "/" + kv.Value.Dir + kv.Value.Name);
                if ( !Directory.Exists(dirPath) )
                {
                    Directory.CreateDirectory(dirPath);
                }
                if ( File.Exists(filePath) )
                {
                    throw new OrchardException(this.T("{2} for {0} already exists in target module {1}.", partName, moduleName, kv.Key));
                }
            }

            // Generating files from templates
            foreach ( var kv in settingsFilesInfo )
            {
                var outputPath = HostingEnvironment.MapPath(extensionFolder + "/" + extensionDescriptor.Id + "/" + kv.Value.Dir + kv.Value.Name);
                FillTemplateAndCreateFile(kv.Key, outputPath, replacements);
                this.AddToModuleProject(kv.Value.Dir + kv.Value.Name, csProjPath, kv.Value.IsCompiled);
                this.Context.Output.WriteLine(this.T("{1} for {0} created successfully", partName, kv.Key));
            }

            this.Context.Output.WriteLine(this.T("Content part {0} type settings created successfully in module {1}", partName, moduleName));
        }

        private static void FillTemplateAndCreateFile(string templateName, string outputPath, IEnumerable<KeyValuePair<string, string>> replacements)
        {
            var data = FillTemplate(templateName, replacements);
            File.WriteAllText(outputPath, data);
        }

        private static string FillTemplate(string templateName, IEnumerable<KeyValuePair<string, string>> replacements)
        {
            var data = File.ReadAllText(templatePath + templateName + ".txt");
            data = replacements.Aggregate(data, (current, s) => current.Replace(string.Format("$${0}$$", s.Key), s.Value));
            return data;
        }

        private void AddToModuleProject(string relPath, string csProjPath, bool isCompiled)
        {
            relPath = relPath.Replace('/', '\\');
            var projectFileText = File.ReadAllText(csProjPath);
            var tagStart = string.Format("<{0} Include", isCompiled ? "Compile" : "Content");

            if ( projectFileText.Contains(tagStart) )
            {
                var compileReference = string.Format("{1}=\"{0}\" />\r\n    ", relPath, tagStart);
                projectFileText = projectFileText.Insert(projectFileText.LastIndexOf(tagStart, StringComparison.Ordinal), compileReference);
            }
            else
            {
                var itemGroupReference = string.Format("</ItemGroup>\r\n  <ItemGroup>\r\n    {1}=\"{0}\" />\r\n  ", relPath, tagStart);
                projectFileText = projectFileText.Insert(projectFileText.LastIndexOf("</ItemGroup>", StringComparison.Ordinal), itemGroupReference);
            }
            File.WriteAllText(csProjPath, projectFileText);
            this.TouchSolution(this.Context.Output);
        }

        private void TouchSolution(TextWriter output)
        {
            var rootWebProjectPath = HostingEnvironment.MapPath("~/Orchard.Web.csproj");
            if ( rootWebProjectPath != null )
            {
                var solutionPath = string.Format("{0}\\Orchard.sln", Directory.GetParent(rootWebProjectPath).Parent.FullName);
                if ( !File.Exists(solutionPath) )
                {
                    output.WriteLine(this.T("Warning: Solution file could not be found at {0}", solutionPath));
                    return;
                }

                try
                {
                    File.SetLastWriteTime(solutionPath, DateTime.Now);
                }
                catch
                {
                    output.WriteLine(this.T("An unexpected error occured while trying to refresh the Visual Studio solution. Please reload it."));
                }
            }
            else
            {
                output.WriteLine(this.T("An unexpected error occured while trying to refresh the Visual Studio solution. Please reload it."));
            }
        }
    }
}