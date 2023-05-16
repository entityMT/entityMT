using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.IntegrationTests.Fakers;

[Schema("Test")]
[Table(nameof(InnerJoinEntity))]
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
    
    public SimpleEntity SimpleEntity { get; set; } = null!;
}