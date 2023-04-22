using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MTConfigurations.Abstractions.Enums;
using MTConfigurations.Abstractions.Providers;
using MtPersistency.CommandFormatters;
using MtPersistency.ConnectionManagers;
using MtPersistency.Handlers;

namespace MtPersistency.SqlServer.Handlers
{
    internal sealed class DefaultSqlServerInsertHandler : IInsertHandler
    {
        private readonly IInsertFormatter _insertFormatter;
        private readonly IColumnsProvider _columnsProvider;
        private readonly IColumnValueProvider _columnValueProvider;
        private readonly IDbConnectionPersistencyManager _dbConnectionPersistencyManager;
        
        public DefaultSqlServerInsertHandler(
            IInsertFormatter insertFormatter,
            IColumnsProvider columnsProvider,
            IColumnValueProvider columnValueProvider,
            IDbConnectionPersistencyManager dbConnectionPersistencyManager)
        {
            _insertFormatter = insertFormatter;
            _columnsProvider = columnsProvider;
            _columnValueProvider = columnValueProvider;
            _dbConnectionPersistencyManager = dbConnectionPersistencyManager;
        }
        
        public void Insert(object source)
        {
            var command = _insertFormatter.FormatCommand(source);
            
            var columns =
                _columnsProvider.GetColumns(source).Where(col => col.ValueGenerated == ValueGenerated.Manual);

            var parameters = new Dictionary<string, object>();

            foreach (var column in columns)
            {
                parameters.Add(column.Name, _columnValueProvider.GetValue(source, column));
            }

            _dbConnectionPersistencyManager.Execute(command, parameters);
        }

        public async Task InsertAsync(object source, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var command = _insertFormatter.FormatCommand(source);
            
            var columns =
                _columnsProvider.GetColumns(source).Where(col => col.ValueGenerated == ValueGenerated.Manual);

            var parameters = new Dictionary<string, object>();

            foreach (var column in columns)
            {
                parameters.Add(column.Name, _columnValueProvider.GetValue(source, column));
            }

            await _dbConnectionPersistencyManager.ExecuteAsync(command, parameters, cancellationToken);
        }
    }
}