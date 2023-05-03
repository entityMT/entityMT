using System.Data;
using System.Data.SqlClient;
using MtTenants.Abstractions;
using MTQueries.Abstractions;

namespace MTQueries.SqlServer
{
    public sealed class DefaultQueryHandler<T> : IQueryHandler<T>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IDataTranscriptor<T> _dataTranscriptor;

        public DefaultQueryHandler(
            ITenantProvider tenantProvider,
            IConnectionStringProvider connectionStringProvider,
            IDataTranscriptor<T> dataTranscriptor)
        {
            var tenant = tenantProvider.GetTenant();
            var connectionString = connectionStringProvider.GetConnectionString(tenant);

            _sqlConnection = new SqlConnection(connectionString);
            _dataTranscriptor = dataTranscriptor;
        }

        public IEnumerable<T> Handle(IQuery query)
        {
            try
            {
                if (_sqlConnection.State != ConnectionState.Open)
                    _sqlConnection.Open();

                var cmd = _sqlConnection.CreateCommand();
                cmd.CommandText = query.Content;

                foreach (var queryParameter in query.Parameters)
                {
                    cmd.Parameters.AddWithValue(
                        $"@{queryParameter.Key}",
                        queryParameter.Value);
                }

                var dataTable = new DataTable();
                var dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dataTable);

                return _dataTranscriptor.Transcript(dataTable);
            }
            catch
            {
                throw;
            }
            finally
            {
                if(_sqlConnection.State == ConnectionState.Open)
                    _sqlConnection.Close();
            }
        }

        public async Task<IEnumerable<T>> HandleAsync(IQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            try
            {
                if (_sqlConnection.State != ConnectionState.Open)
                    await _sqlConnection.OpenAsync(cancellationToken);

                var cmd = _sqlConnection.CreateCommand();
                cmd.CommandText = query.Content;

                foreach (var queryParameter in query.Parameters)
                {
                    cmd.Parameters.AddWithValue(
                        $"@{queryParameter.Key}",
                        queryParameter.Value);
                }

                var dataTable = new DataTable();
                var dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dataTable);

                return _dataTranscriptor.Transcript(dataTable);
            }
            catch
            {
                throw;
            }
            finally
            {
                if(_sqlConnection.State == ConnectionState.Open)
                    await _sqlConnection.CloseAsync();
            }
        }

        public void Dispose()
        {
            _sqlConnection.Dispose();
        }
    }
}