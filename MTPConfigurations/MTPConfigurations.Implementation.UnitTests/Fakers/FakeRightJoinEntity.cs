using MTPConfigurations.Abstractions.Attributes;
using MTPConfigurations.Abstractions.Enums;

namespace MTPConfigurations.Implementation.UnitTests.Fakers
{
    [Table("FakeRightJoinTable")]
    [Schema("dbo")]
    public sealed class FakeRightJoinEntity
    {
        [Join(JoinType.Right)]
        public string Name { get; set; } = null!;
    }
}