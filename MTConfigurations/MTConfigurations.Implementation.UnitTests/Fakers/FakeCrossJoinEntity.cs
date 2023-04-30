using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace MTConfigurations.Implementation.UnitTests.Fakers
{
    [System.ComponentModel.DataAnnotations.Schema.Table("FakeCrossJoinTable")]
    [Schema("dbo")]
    public sealed class FakeCrossJoinEntity
    {
        [Join(JoinType.Cross)]
        public string Name { get; set; } = null!;
    }
}