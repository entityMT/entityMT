using MTConfigurations.Abstractions.Attributes;

namespace MTConfigurations.Implementation.UnitTests.Fakers
{
    [Schema("dbo")]
    [Table("KeyWithoutColumn")]
    public sealed class FakeKeyWithoutColumnEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}