using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTConfigurations.Implementation.UnitTests.Fakers
{
    [Table("FakeLeftJoinTable")]
    [Schema("dbo")]
    public sealed class FakeLeftJoinEntity
    {
        [Join(JoinType.Left)]
        public string Name { get; set; } = null!;
    }
}