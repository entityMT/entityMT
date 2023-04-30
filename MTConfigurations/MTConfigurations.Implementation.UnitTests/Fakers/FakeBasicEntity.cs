using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTConfigurations.Implementation.UnitTests.Fakers
{
    [Table("FakeTable")]
    [Schema("dbo")]
    public sealed class FakeBasicEntity
    {
        [Key]
        [Column("Id", ValueGenerated.Automatic)]
        public Guid Id { get; set; }
        [Column("FieldInteger", ValueGenerated.Manual)]
        public int FieldInteger { get; set; }
        [Column("FieldString", ValueGenerated.Manual)]
        public string FieldString { get; set; } = null!;
        [Column("FieldDate", ValueGenerated.Manual)]
        public DateTime FieldDate { get; set; }
        [Column("FieldBoolean", ValueGenerated.Manual)]
        public bool FieldBoolean { get; set; }
    }
}