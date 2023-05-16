using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace Queries.SqlServer.Entities;

[Schema("dbo")]
[Table("Tutor")]
public sealed class Tutor
{
    [Key]
    [Column("tutorId", ValueGenerated.Automatic)]
    public int Id { get; set; }

    [Column("tutorName", ValueGenerated.Manual)]
    public string Name { get; set; } = null!;
    
    [Column("document", ValueGenerated.Manual)]
    public string Document { get; set; } = null!;
}