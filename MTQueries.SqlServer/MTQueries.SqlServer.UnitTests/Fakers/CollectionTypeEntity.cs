using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.UnitTests.Fakers;

[Table("CollectionTypeEntity")]
public sealed class CollectionTypeEntity
{
    [Key]
    [Column("Id", ValueGenerated.Manual)]
    public string Id { get; set; } = null!;

    public IEnumerable<SimpleEntity> Simplies { get; set; } = null!;
}