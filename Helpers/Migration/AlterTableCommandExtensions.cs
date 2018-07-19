namespace Orchard.Tools.Helpers.Migration {
    using System;
    using System.Linq.Expressions;
    using Data.Migration.Schema;
    using Reflection;
    using Strings;
    using Utility;

    public static class AlterTableCommandExtensions
    {
        public static string CreateIndexKeyName<TRecord>(Expression<Func<TRecord, object>> expression)
        {
            var property = ReflectOn<TRecord>.NameOf(expression);
            return string.Format("IX_{0}_{1}", typeof(TRecord).FullName, property);
        }

        public static void CreateIndexForSingleColumn<TRecord>(this AlterTableCommand self, Expression<Func<TRecord, object>> expression, string indexNameOverride = null)
        {
            var property = ReflectOn<TRecord>.NameOf(expression);
            var indexName = indexNameOverride.HasValue() ? indexNameOverride : CreateIndexKeyName<TRecord>(expression);

            self.CreateIndex(indexName, new[] { property });
        }

        public static void AddColumnFor<TColumn, TRecord>(this AlterTableCommand self, Expression<Func<TRecord, object>> expression, Action<AddColumnCommand> column = null)
        {
#pragma warning disable CS0618 // We need enum support here
            var property = ObjectExtensions.PropertyName(expression);
#pragma warning restore CS0618 // Type or member is obsolete
            self.AddColumn<TColumn>(property, column);
        }

        public static void AddColumnForForeignKey<T>(this AlterTableCommand self, Action<AddColumnCommand> column = null, string columnNameOverride = null)
        {
            var recordType = typeof(T);
            var columnName = columnNameOverride.HasValue() ? columnNameOverride : recordType.Name + "_Id";
            self.AddColumn<int>(columnName, column);
        }
    }
}