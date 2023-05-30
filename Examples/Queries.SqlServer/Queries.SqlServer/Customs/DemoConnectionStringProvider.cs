using MtTenants.Abstractions;

namespace Queries.SqlServer.Customs;

public sealed class DemoConnectionStringProvider : IConnectionStringProvider
{
    public string GetConnectionString(ITenant tenant)
    {
        return "Server=127.0.0.1;Initial Catalog=Examples;User ID=SA;Password=EntityMT2023*;TrustServerCertificate=True;";
    }
}