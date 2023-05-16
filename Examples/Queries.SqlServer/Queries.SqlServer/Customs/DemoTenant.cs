using MtTenants.Abstractions;

namespace Queries.SqlServer.Customs;

public sealed class DemoTenant : ITenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}