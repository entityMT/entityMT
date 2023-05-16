using MtTenants.Abstractions;

namespace Queries.SqlServer.Customs;

public sealed class DemoTenantProvider : ITenantProvider
{
    public ITenant GetTenant()
    {
        return new DemoTenant()
        {
            Id = Guid.NewGuid(),
            Name = "Demo"
        };
    }
}