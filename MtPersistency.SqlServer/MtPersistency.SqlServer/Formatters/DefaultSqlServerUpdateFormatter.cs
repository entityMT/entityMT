using System.Linq;
using System.Text;
using MTConfigurations.Abstractions.Discoveries;
using MTConfigurations.Abstractions.Enums;
using MTConfigurations.Abstractions.Providers;
using MtPersistency.CommandFormatters;
using MtPersistency.Providers;

namespace MtPersistency.SqlServer.Formatters
{
    internal sealed class DefaultSqlServerUpdateFormatter
        : IUpdateFormatter
    {
        private readonly ISchemaProvider _schemaProvider;
        private readonly ITableNameProvider _tableNameProvider;
        private readonly IColumnsProvider _columnsProvider;
        private readonly IParameterPrefixProvider _parameterPrefixProvider;
        private readonly IKeyColumnDiscovery _keyColumnDiscovery;
        
        public DefaultSqlServerUpdateFormatter(
            ISchemaProvider schemaProvider,
            ITableNameProvider tableNameProvider,
            IColumnsProvider columnsProvider,
            IParameterPrefixProvider parameterPrefixProvider,
            IKeyColumnDiscovery keyColumnDiscovery)
        {
            _schemaProvider = schemaProvider;
            _tableNameProvider = tableNameProvider;
            _columnsProvider = columnsProvider;
            _parameterPrefixProvider = parameterPrefixProvider;
            _keyColumnDiscovery = keyColumnDiscovery;
        }
        
        public string FormatCommand(object source)
        {
            var updateStringBuilder = new StringBuilder();

            updateStringBuilder.Append(
                $"UPDATE {_schemaProvider.GetSchema(source)}.{_tableNameProvider.GetTableName(source)} SET ");

            var columns =
                _columnsProvider.GetColumns(source).Where(col => col.ValueGenerated == ValueGenerated.Manual);

            var keyColumnName = _keyColumnDiscovery.Discovery(source).Name;
            
            foreach (var column in columns)
            {
                updateStringBuilder.Append(
                    $"{column.Name} = {_parameterPrefixProvider.GetPrefix()}{column.Name},");
            }

            updateStringBuilder.Remove(updateStringBuilder.Length - 1, 1);
            updateStringBuilder.Append(" WHERE ");
            updateStringBuilder.Append($"{keyColumnName} = {_parameterPrefixProvider.GetPrefix()}{keyColumnName}");
            
            return updateStringBuilder.ToString();
        }
    }
}