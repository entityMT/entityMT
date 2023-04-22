using MTPConfigurations.Abstractions.Attributes;
using MTPConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.UnitTests.Fakers;

[Schema("dbo")]
[Table("CrossJoinEntity")]
public sealed class CrossJoinEntity
{
    [Key]
    [Column("Id", ValueGenerated.Manual)]
    public Guid Id { get; set; }
    
    [ForeignKey(typeof(SimpleEntity))]
    [Column("SimpleEntityId", ValueGenerated.Manual)]
    [Join(JoinType.Cross)]
    public Guid SimpleEntityId { get; set; }

    public SimpleEntity SimpleEntity { get; set; } = null!;
}