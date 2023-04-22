using System.Text;
using MTConfigurations.Abstractions.Discoveries;
using MTConfigurations.Abstractions.Providers;
using MtPersistency.CommandFormatters;
using MtPersistency.Providers;

namespace MtPersistency.SqlServer.Formatters
{
    internal sealed class DefaultSqlServerDeleteFormatter
        : IDeleteFormatter
    {
        private readonly ISchemaProvider _schemaProvider;
        private readonly ITableNameProvider _tableNameProvider;
        private readonly IParameterPrefixProvider _parameterPrefixProvider;
        private readonly IKeyColumnDiscovery _keyColumnDiscovery;
        
        public DefaultSqlServerDeleteFormatter(
            ISchemaProvider schemaProvider,
            ITableNameProvider tableNameProvider,
            IParameterPrefixProvider parameterPrefixProvider,
            IKeyColumnDiscovery keyColumnDiscovery)
        {
            _schemaProvider = schemaProvider;
            _tableNameProvider = tableNameProvider;
            _parameterPrefixProvider = parameterPrefixProvider;
            _keyColumnDiscovery = keyColumnDiscovery;
        }
        
        public string FormatCommand(object source)
        {
            var deleteStringBuilder = new StringBuilder();

            deleteStringBuilder.Append(
                $"DELETE FROM {_schemaProvider.GetSchema(source)}.{_tableNameProvider.GetTableName(source)}");

            deleteStringBuilder.Append(" WHERE ");

            var keyColumnName = _keyColumnDiscovery.Discovery(source).Name;

            deleteStringBuilder.Append($"{keyColumnName} = {_parameterPrefixProvider.GetPrefix()}{keyColumnName}");

            return deleteStringBuilder.ToString();
        }
    }
}