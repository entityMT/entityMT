using MTPConfigurations.Abstractions.Attributes;
using MTPConfigurations.Abstractions.Enums;

namespace MTPConfigurations.Implementation.UnitTests.Fakers
{
    [Table("FakeLeftJoinTable")]
    [Schema("dbo")]
    public sealed class FakeLeftJoinEntity
    {
        [Join(JoinType.Left)]
        public string Name { get; set; } = null!;
    }
}