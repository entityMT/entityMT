using System;
using MTPConfigurations.Abstractions.Attributes;

namespace MTPConfigurations.Implementation.UnitTests.Fakers
{
    [Schema("dbo")]
    [Table("KeyWithoutColumn")]
    public sealed class FakeKeyWithoutColumnEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}