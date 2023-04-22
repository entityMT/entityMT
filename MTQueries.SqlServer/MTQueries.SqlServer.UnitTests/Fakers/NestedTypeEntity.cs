using MTPConfigurations.Abstractions.Attributes;
using MTPConfigurations.Abstractions.Enums;

namespace MTQueries.SqlServer.UnitTests.Fakers;

[Table("NestedTypeEntity")]
public sealed class NestedTypeEntity
{
    [Key]
    [Column("Id", ValueGenerated.Automatic)]
    public double Id { get; set; }
    
    [Column("IsNested", ValueGenerated.Manual)]
    public bool IsNested { get; set; }
    
    [Column("Price", ValueGenerated.Manual)]
    public decimal Price { get; set; }
    
    [Column("FeesTax", ValueGenerated.Manual)]
    public float FeesTax { get; set; }
    
    [Column("Quantity", ValueGenerated.Manual)]
    public long Quantity { get; set; }
    
    public SimpleEntity SimpleEntity { get; set; } = null!;
}