namespace Orchard.Tools.Helpers.Migration {
    using System;
    using System.Globalization;
    using ContentManagement.MetaData.Builders;
    using ContentPicker.Fields;
    using Core.Common.Models;
    using Core.Containers.Models;
    using Core.Title.Models;
    using Data.Migration.Schema;
    using Fields.Settings;
    using Strings;

    public static class MigrationExtensionHelpers {
        public enum Flavour {
            Html,
            Markdown,
            Text,
            Textarea
        }

        public static ContentPartDefinitionBuilder WithContentPickerField(
            this ContentPartDefinitionBuilder builder,
            string name,
            string hint = null,
            bool isRequired = false,
            bool allowMultiple = false,
            bool showContentTab = true,
            string[] displayedContentTypes = null) {
            var displayName = name.SplitCamel();
            var contentTypes = displayedContentTypes != null ? string.Join(",", displayedContentTypes) : null;

            return builder.WithField(
                name,
                cfg => cfg.OfType(typeof(ContentPickerField).Name)
                    .WithDisplayName(displayName)
                    .WithSetting("ContentPickerFieldSettings.Hint", hint)
                    .WithSetting("ContentPickerFieldSettings.Required", isRequired.ToString(CultureInfo.InvariantCulture))
                    .WithSetting("ContentPickerFieldSettings.Multiple", allowMultiple.ToString(CultureInfo.InvariantCulture))
                    .WithSetting("ContentPickerFieldSettings.ShowContentTab", showContentTab.ToString(CultureInfo.InvariantCulture))
                    .WithSetting("ContentPickerFieldSettings.DisplayedContentTypes", contentTypes));
        }

        public static ContentPartDefinitionBuilder WithMediaLibraryPickerField(
            this ContentPartDefinitionBuilder builder,
            string name,
            bool required = true,
            bool multiple = false,
            string hint = "",
            string displayedContentTypes = "jpg jpeg png gif") {
            var displayName = name.SplitCamel();

            // default implementation of Media library picker field - create overloads for more options
            return builder.WithField(
                name,
                fieldBuilder =>
                    fieldBuilder.OfType("MediaLibraryPickerField")
                        .WithDisplayName(displayName)
                        .WithSetting("MediaLibraryPickerFieldSettings.Required", required.ToString(CultureInfo.InvariantCulture))
                        .WithSetting("MediaLibraryPickerFieldSettings.DisplayedContentTypes", displayedContentTypes)
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", multiple.ToString(CultureInfo.InvariantCulture))
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", hint));
        }

        public static ContentPartDefinitionBuilder WithTaxonomyField(
            this ContentPartDefinitionBuilder builder,
            string fieldName,
            string taxonomyName,
            bool required = false,
            bool leavesOnly = false,
            bool singleChoice = false,
            bool autocomplete = false,
            bool allowCustomTerms = false,
            string hint = "") {
            var displayName = fieldName.SplitCamel();

            // default implementation of Media library picker field - create overloads for more options
            return builder.WithField(
                fieldName,
                fieldBuilder =>
                    fieldBuilder.OfType("TaxonomyField")
                        .WithDisplayName(displayName)
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", taxonomyName)
                        .WithSetting("TaxonomyFieldSettings.Required", required.ToString(CultureInfo.InvariantCulture))
                        .WithSetting("TaxonomyFieldSettings.LeavesOnly", leavesOnly.ToString(CultureInfo.InvariantCulture))
                        .WithSetting("TaxonomyFieldSettings.SingleChoice", singleChoice.ToString(CultureInfo.InvariantCulture))
                        .WithSetting("TaxonomyFieldSettings.Autocomplete", autocomplete.ToString(CultureInfo.InvariantCulture))
                        .WithSetting("TaxonomyFieldSettings.AllowCustomTerms", allowCustomTerms.ToString(CultureInfo.InvariantCulture))
                        .WithSetting("TaxonomyFieldSettings.Hint", hint));
        }

        public static ContentPartDefinitionBuilder WithTextField(
            this ContentPartDefinitionBuilder builder,
            string name,
            Flavour flavor,
            bool required = true,
            string hint = "") {
            var strFlavor = flavor.ToString().SplitCamel();

            return builder.WithField(
                name,
                fieldBuilder =>
                    fieldBuilder.OfType("TextField")
                        .WithSetting("TextFieldSettings.Required", required.ToString(CultureInfo.InvariantCulture))
                        .WithSetting("TextFieldSettings.Flavor", strFlavor)
                        .WithSetting("TextFieldSettings.Hint", hint));
        }

        public static ContentPartDefinitionBuilder WithBooleanField(
            this ContentPartDefinitionBuilder builder,
            string name,
            bool @default,
            string hint = "") {
            return builder.WithField(
                name,
                fieldBuilder =>
                    fieldBuilder.OfType("BooleanField")
                        .WithSetting("BooleanFieldSettings.Hint", hint)
                        .WithSetting("BooleanFieldSettings.DefaultValue", @default.ToString(CultureInfo.InvariantCulture)));
        }

        public static ContentPartDefinitionBuilder WithLinkField(
            this ContentPartDefinitionBuilder builder,
            string fieldName,
            string displayName = null,
            bool required = false,
            TargetMode targetMode = TargetMode.None,
            LinkTextMode linkTextMode = LinkTextMode.Optional,
            string hint = "") {
            return builder.WithField(
                fieldName,
                fieldBuilder => {
                    fieldBuilder.OfType("LinkField")
                        .WithSetting("LinkFieldSettings.Hint", hint)
                        .WithSetting("LinkFieldSettings.Required", required.ToString(CultureInfo.InvariantCulture))
                        .WithSetting("LinkFieldSettings.TargetMode", targetMode.ToString())
                        .WithSetting("LinkFieldSettings.LinkTextMode", linkTextMode.ToString());

                    if ( displayName.HasValue() ) {
                        fieldBuilder.WithDisplayName(displayName);
                    }
                });
        }

        public static ContentTypeDefinitionBuilder WithAutoroutePart(
            this ContentTypeDefinitionBuilder builder,
            string pathPrefix = "",
            bool automaticAdjustmentOnEdit = false) {
            var pattern = string.Format(
                "[{{Title:'{0}/Title', Pattern: '{0}/{{Content.Slug}}', Description: '{0}/my-page'}}]",
                pathPrefix.TrimEnd('/'));

            return builder.WithPart(
                "AutoroutePart",
                partBuilder => partBuilder
                    .WithSetting("AutorouteSettings.PatternDefinitions", pattern)
                    .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", automaticAdjustmentOnEdit.ToString()));
        }

        public static ContentTypeDefinitionBuilder WithBodyPart(
            this ContentTypeDefinitionBuilder builder,
            Flavour defaultFlavour = Flavour.Html) {
            return builder.WithPart(
                typeof(BodyPart).Name,
                partBuilder => partBuilder.WithSetting("BodyTypePartSettings.Flavor", defaultFlavour.ToString()));
        }

        public static ContentTypeDefinitionBuilder WithCommonPart(this ContentTypeDefinitionBuilder builder, bool showOwnerEditor = false) {
            return builder.WithPart(typeof(CommonPart).Name)
                .WithSetting("OwnerEditorSettings.ShowOwnerEditor", showOwnerEditor.ToString(CultureInfo.InvariantCulture).ToLower());
        }

        public static ContentTypeDefinitionBuilder WithTitlePart(this ContentTypeDefinitionBuilder builder) {
            return builder.WithPart(typeof(TitlePart).Name);
        }

        public static ContentTypeDefinitionBuilder WithContainerPart(
            this ContentTypeDefinitionBuilder builder,
            string[] restrictedItemContentTypes,
            bool useCondensedViewAsDefault = false,
            bool enablePositioning = false,
            bool displayContainerEditor = false) {
            var restrictContentTypes = restrictedItemContentTypes != null;

            return builder.WithPart(
                typeof(ContainerPart).Name,
                cfg => cfg
                    .WithSetting("ContainerTypePartSettings.AdminListViewName", useCondensedViewAsDefault ? "Condensed" : "Default")
                    .WithSetting("ContainerTypePartSettings.EnablePositioning", enablePositioning.ToString())
                    .WithSetting("ContainerTypePartSettings.DisplayContainerEditor", displayContainerEditor.ToString())
                    .WithSetting("ContainerTypePartSettings.RestrictItemContentTypes", restrictContentTypes.ToString())
                    .WithSetting("ContainerTypePartSettings.RestrictedItemContentTypes", restrictContentTypes ? string.Join(",", restrictedItemContentTypes) : string.Empty));
        }

        public static ContentTypeDefinitionBuilder WithContainablePart(this ContentTypeDefinitionBuilder builder, bool showContainerPicker = false, bool showPositionEditor = false) {
            return builder.WithPart(
                typeof(ContainablePart).Name,
                cfg => cfg
                    .WithSetting("ContainableTypePartSettings.ShowContainerPicker", showContainerPicker.ToString())
                    .WithSetting("ContainableTypePartSettings.ShowPositionEditor", showPositionEditor.ToString()));
        }

        /// <summary>Creates a foreignkey based on some defaults</summary>
        /// <param name="schemaBuilder">The schemaBuilder</param>
        /// <param name="srcRecordType">The type of the source contentTypeRecord, which will resolve to the source table</param>
        /// <param name="destRecordType">The type of the destination contentTypeRecord, which will resolve to the destination table</param>
        /// <param name="srcColumn">Overrides the source column name. Defaults to 'DestinationContentTypeRecord_Id'</param>
        /// <param name="destColumn">Overrides the destination column name</param>
        /// <param name="destModule">Optional. Specify the full module name if the destination table is from another module</param>
        public static void CreateForeignKey(this SchemaBuilder schemaBuilder, Type srcRecordType, Type destRecordType, string srcColumn = null, string destColumn = "Id", string destModule = null) {
            var foreignKeyName = CreateForeignKeyName(srcRecordType, destRecordType, srcColumn, destColumn, destModule);
            var srcTableName = srcRecordType.Name;
            var destTableName = destRecordType.Name;
            srcColumn = string.IsNullOrEmpty(srcColumn) ? string.Format("{0}_Id", destTableName) : srcColumn;

            if ( !string.IsNullOrEmpty(destModule) ) {
                schemaBuilder.CreateForeignKey(
                    foreignKeyName,
                    srcTableName,
                    new[] { srcColumn },
                    destModule,
                    destTableName,
                    new[] { destColumn });
            }
            else {
                schemaBuilder.CreateForeignKey(
                    foreignKeyName,
                    srcTableName,
                    new[] { srcColumn },
                    destTableName,
                    new[] { destColumn });
            }
        }

        public static string CreateForeignKeyName(Type srcRecordType, Type destRecordType, string srcColumn = null, string destColumn = "Id", string destModule = null) {
            var srcTableName = srcRecordType.Name;
            var destTableName = destRecordType.Name;
            srcColumn = string.IsNullOrEmpty(srcColumn) ? string.Format("{0}_Id", destTableName) : srcColumn;

            var foreignKeyName = string.Format("FK__{0}{1}_{2}{3}", srcTableName, srcColumn, destTableName, destColumn != "Id" ? destColumn : string.Empty);
            return foreignKeyName;
        }
    }
}