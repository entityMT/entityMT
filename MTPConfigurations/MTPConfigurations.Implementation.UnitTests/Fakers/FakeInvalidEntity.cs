using System;

namespace MTPConfigurations.Implementation.UnitTests.Fakers
{
    public sealed class FakeInvalidEntity
    {
        public Guid Id { get; set; }
        public string Field { get; set; } = null!;
    }
}