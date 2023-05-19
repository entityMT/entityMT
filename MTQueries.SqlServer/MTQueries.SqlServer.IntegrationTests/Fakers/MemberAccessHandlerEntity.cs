using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.IntegrationTests.Fakers;

[Schema("dbo")]
[Table(nameof(MemberAccessHandlerEntity))]
public sealed class MemberAccessHandlerEntity
{
    [Column("Date", ValueGenerated.Manual)]
    public DateTime Date { get; set; }
    
    [Column("Int", ValueGenerated.Manual)]
    public int Int { get; set; }
    
    [Column("Bool", ValueGenerated.Manual)]
    public bool Bool { get; set; }
    
    [Column("Byte", ValueGenerated.Manual)]
    public byte Byte { get; set; }
    
    [Column("Char", ValueGenerated.Manual)]
    public char Char { get; set; }
    
    [Column("Decimal", ValueGenerated.Manual)]
    public decimal Decimal { get; set; }
    
    [Column("Double", ValueGenerated.Manual)]
    public double Double { get; set; }
    
    [Column("Long", ValueGenerated.Manual)]
    public long Long { get; set; }

    [Column("String", ValueGenerated.Manual)]
    public string String { get; set; } = null!;
    
    [Column("Guid", ValueGenerated.Manual)]
    public Guid Guid { get; set; }
}