using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.IntegrationTests.Fakers;

[Schema("Test")]
[Table(nameof(SimpleEntity))]
public sealed class SimpleEntity
{
    [Key]
    [Column("Id", ValueGenerated.Manual)]
    public Guid Id { get; set; }
    
    [Column("Name", ValueGenerated.Manual)]
    public string Name { get; set; } = null!;

    [Column("Year", ValueGenerated.Manual)]
    public int Years { get; set; }
}