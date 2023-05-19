using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.IntegrationTests.Fakers;

[Schema("dbo")]
[Table(nameof(MoreThanOneJoinEntity))]
public sealed class MoreThanOneJoinEntity
{
    [Key]
    [Column("id", ValueGenerated.Manual)]
    public Guid Id { get; set; }
    
    [Column("name", ValueGenerated.Manual)]
    public string Name { get; set; } = null!;

    [Column("innerJoinId", ValueGenerated.Manual)]
    [ForeignKey(typeof(InnerJoinEntity))]
    [Join(JoinType.Inner)]
    public Guid InnerJoinId { get; set; }
    
    public InnerJoinEntity InnerJoinEntity { get; set; } = null!;
}