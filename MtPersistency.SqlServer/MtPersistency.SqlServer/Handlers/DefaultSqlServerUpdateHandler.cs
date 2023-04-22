using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MTConfigurations.Abstractions.Discoveries;
using MTConfigurations.Abstractions.Enums;
using MTConfigurations.Abstractions.Providers;
using MtPersistency.CommandFormatters;
using MtPersistency.ConnectionManagers;
using MtPersistency.Handlers;

namespace MtPersistency.SqlServer.Handlers
{
    internal sealed class DefaultSqlServerUpdateHandler : IUpdateHandler
    {
        private readonly IUpdateFormatter _updateFormatter;
        private readonly IColumnsProvider _columnsProvider;
        private readonly IColumnValueProvider _columnValueProvider;
        private readonly IKeyColumnDiscovery _keyColumnDiscovery;
        private readonly IDbConnectionPersistencyManager _dbConnectionPersistencyManager;
        
        public DefaultSqlServerUpdateHandler(
            IUpdateFormatter updateFormatter,
            IColumnsProvider columnsProvider,
            IColumnValueProvider columnValueProvider,
            IKeyColumnDiscovery keyColumnDiscovery,
            IDbConnectionPersistencyManager dbConnectionPersistencyManager)
        {
            _dbConnectionPersistencyManager = dbConnectionPersistencyManager;
            _updateFormatter = updateFormatter;
            _columnsProvider = columnsProvider;
            _columnValueProvider = columnValueProvider;
            _keyColumnDiscovery = keyColumnDiscovery;
        }
        
        public void Update(object source)
        {
            var command = _updateFormatter.FormatCommand(source);
            
            var columns =
                _columnsProvider.GetColumns(source).Where(col => col.ValueGenerated == ValueGenerated.Manual);

            var parameters = new Dictionary<string, object>();

            foreach (var column in columns)
            {
                parameters.Add(column.Name, _columnValueProvider.GetValue(source,column));
            }

            var keyColumn = _keyColumnDiscovery.Discovery(source);

            parameters.Add(keyColumn.Name,_columnValueProvider.GetValue(source, keyColumn));

            _dbConnectionPersistencyManager.Execute(command, parameters);
        }

        public async Task UpdateAsync(object source, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var command = _updateFormatter.FormatCommand(source);
            
            var columns =
                _columnsProvider.GetColumns(source).Where(col => col.ValueGenerated == ValueGenerated.Manual);
            
            var parameters = new Dictionary<string, object>();

            foreach (var column in columns)
            {
                parameters.Add(column.Name, _columnValueProvider.GetValue(source,column));
            }
                
             var keyColumn = _keyColumnDiscovery.Discovery(source);

             parameters.Add(keyColumn.Name,_columnValueProvider.GetValue(source, keyColumn));

             await _dbConnectionPersistencyManager.ExecuteAsync(command, parameters, cancellationToken);
        }
    }
}