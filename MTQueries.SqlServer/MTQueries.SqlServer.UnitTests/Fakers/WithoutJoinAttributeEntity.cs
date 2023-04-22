using MTPConfigurations.Abstractions.Attributes;
using MTPConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.UnitTests.Fakers;

[Schema("dbo")]
[Table("WithoutJoinAttributeEntity")]
public sealed class WithoutJoinAttributeEntity
{
    [Key]
    [Column("Id", ValueGenerated.Manual)]
    public Guid Id { get; set; }
    
    [ForeignKey(typeof(SimpleEntity))]
    [Column("SimpleEntityId", ValueGenerated.Manual)]
    public Guid SimpleEntityId { get; set; }

    public SimpleEntity SimpleEntity { get; set; } = null!;
}