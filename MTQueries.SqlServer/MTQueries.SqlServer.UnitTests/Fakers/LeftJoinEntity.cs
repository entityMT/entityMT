using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.UnitTests.Fakers;

[Schema("dbo")]
[Table("LeftJoinEntity")]
public sealed class LeftJoinEntity
{
    [Key]
    [Column("Id", ValueGenerated.Manual)]
    public Guid Id { get; set; }
    
    [Column("Description", ValueGenerated.Manual)]
    public string Description { get; set; } = null!;

    [ForeignKey(typeof(SimpleEntity))]
    [Join(JoinType.Left)]
    [Column("SimpleEntityId", ValueGenerated.Manual)]
    public Guid SimpleEntityId { get; set; }
    
    public SimpleEntity SimpleEntity { get; set; } = null!;
}