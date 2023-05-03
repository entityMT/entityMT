using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.UnitTests.Fakers;

[Schema("dbo")]
[Table("InnerJoinEntity")]
public sealed class InnerJoinEntity
{
    [Key]
    [Column("Id", ValueGenerated.Manual)]
    public Guid Id { get; set; }
    
    [Column("Description", ValueGenerated.Manual)]
    public string Description { get; set; } = null!;
    
    [Column("SimpleEntityId", ValueGenerated.Manual)]
    [ForeignKey(typeof(SimpleEntity))]
    [Join(JoinType.Inner)]
    public Guid SimpleEntityId { get; set; }
    
    public IEnumerable<SimpleEntity> SimpleEntities { get; set; } = null!;
}