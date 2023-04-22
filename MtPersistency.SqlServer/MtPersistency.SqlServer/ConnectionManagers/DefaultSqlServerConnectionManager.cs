using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using MtPersistency.ConnectionManagers;
using MtPersistency.Providers;
using MtPersistency.UnitOfWork;
using MtTenants.Abstractions;

namespace MtPersistency.SqlServer.ConnectionManagers
{
    internal sealed class DefaultSqlServerConnectionManager : IDbConnectionPersistencyManager, IUnitOfWork
    {
        private readonly SqlConnection _sqlConnection;
        private DbTransaction? _transaction;
        private readonly IParameterPrefixProvider _parameterPrefixProvider;
        
        public DefaultSqlServerConnectionManager(
            ITenantProvider tenantProvider,
            IConnectionStringProvider connectionStringProvider,
            IParameterPrefixProvider parameterPrefixProvider)
        {
            _parameterPrefixProvider = parameterPrefixProvider;
            
            var tenant = tenantProvider.GetTenant();
            var connectionString = connectionStringProvider.GetConnectionString(tenant);

            _sqlConnection = new SqlConnection(connectionString);
        }
        
        public void Dispose()
        {
            _transaction?.Dispose();
            _sqlConnection.Dispose();
        }

        public void Open()
        {
            _sqlConnection.Open();
        }

        public async Task OpenAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            await _sqlConnection.OpenAsync(cancellationToken);
        }

        public void Execute(string command, IDictionary<string, object> parameters)
        {
            try
            {
                if (_sqlConnection.State != ConnectionState.Open)
                    _sqlConnection.Open();

                var cmd = _sqlConnection.CreateCommand();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = command;

                foreach (var key in parameters.Keys)
                {
                    cmd.Parameters.AddWithValue(
                        $"{_parameterPrefixProvider.GetPrefix()}{key}",
                        parameters[key]);
                }

                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_sqlConnection.State == ConnectionState.Open && _transaction == default)
                    _sqlConnection.Close();    
            }
        }

        public async Task ExecuteAsync(string command, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            try
            {
                if (_sqlConnection.State != ConnectionState.Open)
                    await _sqlConnection.OpenAsync(cancellationToken);

                var cmd = _sqlConnection.CreateCommand();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = command;

                foreach (var key in parameters.Keys)
                {
                    cmd.Parameters.AddWithValue(
                        $"{_parameterPrefixProvider.GetPrefix()}{key}",
                        parameters[key]);
                }

                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_sqlConnection.State == ConnectionState.Open && _transaction == default)
                    await _sqlConnection.CloseAsync();    
            }
        }

        public void BeginTransaction()
        {
            if (_sqlConnection.State != ConnectionState.Open)
                _sqlConnection.Open();

            _transaction = _sqlConnection.BeginTransaction();
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (_sqlConnection.State != ConnectionState.Open)
                await _sqlConnection.OpenAsync(cancellationToken);

            _transaction = await _sqlConnection.BeginTransactionAsync(cancellationToken);
        }

        public void Commit()
        {
            if (_transaction != default)
            {
                _transaction.Commit();
                _sqlConnection.Close();
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (_transaction != default)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _sqlConnection.CloseAsync();
            }
        }
    }
}