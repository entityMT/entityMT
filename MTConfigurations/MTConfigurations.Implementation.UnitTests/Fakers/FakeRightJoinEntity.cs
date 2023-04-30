using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTConfigurations.Implementation.UnitTests.Fakers
{
    [Table("FakeRightJoinTable")]
    [Schema("dbo")]
    public sealed class FakeRightJoinEntity
    {
        [Join(JoinType.Right)]
        public string Name { get; set; } = null!;
    }
}