using MTPConfigurations.Abstractions.Attributes;
using MTPConfigurations.Abstractions.Enums;

namespace MTPConfigurations.Implementation.UnitTests.Fakers
{
    [System.ComponentModel.DataAnnotations.Schema.Table("FakeCrossJoinTable")]
    [Schema("dbo")]
    public sealed class FakeCrossJoinEntity
    {
        [Join(JoinType.Cross)]
        public string Name { get; set; } = null!;
    }
}