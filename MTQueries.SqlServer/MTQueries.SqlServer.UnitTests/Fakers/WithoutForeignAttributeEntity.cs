using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.UnitTests.Fakers;

[Schema("dbo")]
[Table("WithoutForeignAttributeEntity")]
public sealed class WithoutForeignAttributeEntity
{
    [Key]
    [Column("Id", ValueGenerated.Manual)]
    public int Id { get; set; }

    public IEnumerable<SimpleEntity> SimpleEntities { get; set; } = null!;
}