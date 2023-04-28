using Microsoft.Extensions.Configuration;
using MtTenants.Abstractions;

namespace MtTenants.Implementation
{
    public sealed class DefaultConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;
        
        public DefaultConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string GetConnectionString(ITenant tenant)
        {
            string connectionString = _configuration.GetSection($"{tenant.Name}_CONNECTIONSTRING").Value;
            return connectionString;
        }
    }
}