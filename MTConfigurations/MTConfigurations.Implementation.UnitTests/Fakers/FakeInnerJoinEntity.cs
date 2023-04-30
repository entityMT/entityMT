using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTConfigurations.Implementation.UnitTests.Fakers
{
    [Table("FakeInnerJoinTable")]
    [Schema("dbo")]
    public sealed class FakeInnerJoinEntity
    {
        [Join(JoinType.Inner)]
        public string Name { get; set; } = null!;
    }
}