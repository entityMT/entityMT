using System.Linq;
using System.Text;
using MTConfigurations.Abstractions.Enums;
using MTConfigurations.Abstractions.Providers;
using MtPersistency.CommandFormatters;
using MtPersistency.Providers;

namespace MtPersistency.SqlServer.Formatters
{
    internal sealed class DefaultSqlServerInsertFormatter
        : IInsertFormatter
    {
        private readonly ISchemaProvider _schemaProvider;
        private readonly ITableNameProvider _tableNameProvider;
        private readonly IColumnsProvider _columnsProvider;
        private readonly IParameterPrefixProvider _parameterPrefixProvider;
        
        public DefaultSqlServerInsertFormatter(
            ISchemaProvider schemaProvider,
            ITableNameProvider tableNameProvider,
            IColumnsProvider columnsProvider,
            IParameterPrefixProvider parameterPrefixProvider)
        {
            _schemaProvider = schemaProvider;
            _tableNameProvider = tableNameProvider;
            _columnsProvider = columnsProvider;
            _parameterPrefixProvider = parameterPrefixProvider;
        }
        
        public string FormatCommand(object source)
        {
            var insertStringBuilder = new StringBuilder();

            insertStringBuilder.Append(
                $"INSERT INTO {_schemaProvider.GetSchema(source)}.{_tableNameProvider.GetTableName(source)} VALUES(");

            var columns = 
                _columnsProvider.GetColumns(source).Where(col => col.ValueGenerated == ValueGenerated.Manual);

            foreach (var column in columns)
            {
                insertStringBuilder.Append(
                    $"{_parameterPrefixProvider.GetPrefix()}{column.Name},");
            }

            insertStringBuilder.Remove(insertStringBuilder.Length - 1, 1);
            insertStringBuilder.Append(")");
            
            return insertStringBuilder.ToString();
        }
    }
}