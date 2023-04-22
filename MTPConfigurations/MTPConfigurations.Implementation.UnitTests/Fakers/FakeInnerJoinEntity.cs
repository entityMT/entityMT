using MTPConfigurations.Abstractions.Attributes;
using MTPConfigurations.Abstractions.Enums;

namespace MTPConfigurations.Implementation.UnitTests.Fakers
{
    [Table("FakeInnerJoinTable")]
    [Schema("dbo")]
    public sealed class FakeInnerJoinEntity
    {
        [Join(JoinType.Inner)]
        public string Name { get; set; } = null!;
    }
}