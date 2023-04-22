using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MTConfigurations.Abstractions.Discoveries;
using MTConfigurations.Abstractions.Providers;
using MtPersistency.CommandFormatters;
using MtPersistency.ConnectionManagers;
using MtPersistency.Handlers;

namespace MtPersistency.SqlServer.Handlers
{
    internal sealed class DefaultSqlServerDeleteHandler
        : IDeleteHandler
    {
        private readonly IKeyColumnDiscovery _keyColumnDiscovery;
        private readonly IColumnValueProvider _columnValueProvider;
        private readonly IDeleteFormatter _deleteFormatter;
        private readonly IDbConnectionPersistencyManager _dbConnectionPersistencyManager;
        
        public DefaultSqlServerDeleteHandler(
            IKeyColumnDiscovery keyColumnDiscovery,
            IColumnValueProvider columnValueProvider,
            IDeleteFormatter deleteFormatter,
            IDbConnectionPersistencyManager dbConnectionPersistencyManager)
        {
            _keyColumnDiscovery = keyColumnDiscovery;
            _columnValueProvider = columnValueProvider;
            _deleteFormatter = deleteFormatter;
            _dbConnectionPersistencyManager = dbConnectionPersistencyManager;
        }
        
        public void Delete(object source)
        {
            var command = _deleteFormatter.FormatCommand(source);
            var keyColumn = _keyColumnDiscovery.Discovery(source);
            var parameters = new Dictionary<string, object>();
            
            parameters.Add(keyColumn.Name, _columnValueProvider.GetValue(source, keyColumn));

            _dbConnectionPersistencyManager.Execute(command, parameters);
        }

        public async Task DeleteAsync(object source, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var command = _deleteFormatter.FormatCommand(source);
            var keyColumn = _keyColumnDiscovery.Discovery(source);
            var parameters = new Dictionary<string, object>();
            
            parameters.Add(keyColumn.Name, _columnValueProvider.GetValue(source, keyColumn));

            await _dbConnectionPersistencyManager.ExecuteAsync(command, parameters, cancellationToken);
        }
    }
}