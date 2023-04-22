using MTPConfigurations.Abstractions.Attributes;
using MTPConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.UnitTests.Fakers;

[Schema("dbo")]
[Table("RightJoinEntity")]
public sealed class RightJoinEntity
{
    [Key]
    [Column("Id", ValueGenerated.Manual)]
    public Guid Id { get; set; }

    [Column("price", ValueGenerated.Manual)]
    public double Price { get; set; }
    
    [Column("quantity", ValueGenerated.Manual)]
    public int Quantity { get; set; }
    
    [ForeignKey(typeof(SimpleEntity))]
    [Join(JoinType.Right)]
    [Column("SimpleEntityId", ValueGenerated.Manual)]
    public Guid SimpleEntityId { get; set; }
    
    public IEnumerable<SimpleEntity> SimpleEntities { get; set; } = null!;
}