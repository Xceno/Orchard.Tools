namespace Orchard.Tools.Helpers.Migration {
    using System;
    using System.Data;
    using System.Linq.Expressions;
    using Data.Migration.Schema;
    using Reflection;
    using Strings;
    using Utility;

    public static class CreateTableCommandExtensions
    {
        /// <summary>Creates a column with the name of TBaseRecordProperty_Id. This follows the standard orchard conventions.</summary>
        /// <typeparam name="TForeignRecord">The Record-Type of the virtual property.</typeparam>
        /// <typeparam name="TBaseRecord">The ContentPartRecord holding the virtual property pointing to the foreign record.</typeparam>
        /// <param name="self">A CreateTableCommand</param>
        /// <param name="expression">An expression to get the virtual property.</param>
        /// <param name="column">Actions to alter the column further.</param>
        /// <returns>The CreateTableCommand</returns>
        public static CreateTableCommand ColumnForForeignKey<TForeignRecord, TBaseRecord>(this CreateTableCommand self, Expression<Func<TBaseRecord, TForeignRecord>> expression, Action<CreateColumnCommand> column = null)
        {
            var dbType = SchemaUtils.ToDbType(typeof(int));

            var property = ReflectOn<TBaseRecord>.NameOf(expression);
            var columnName = string.Format("{0}_Id", property);
            return self.Column(columnName, dbType, column);
        }

#pragma warning disable CS0618 // We need enum support here
        public static CreateTableCommand ColumnFor<TColumn, TRecord>(this CreateTableCommand self, Expression<Func<TRecord, object>> expression, Action<CreateColumnCommand> column = null)
        {

            var property = ObjectExtensions.PropertyName(expression);
            return self.Column<TColumn>(property, column);
        }

        public static CreateTableCommand ColumnFor<TRecord>(this CreateTableCommand self, Expression<Func<TRecord, object>> expression, DbType dbType, Action<CreateColumnCommand> column = null)
        {
            var property = ObjectExtensions.PropertyName(expression);
            return self.Column(property, dbType, column);
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}