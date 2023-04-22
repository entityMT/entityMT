using System;
using MtTenants.Abstractions;

namespace MtTenants.Implementation.UnitTests.TestObjects
{
    internal sealed class TenantTest : ITenant
    {
        public TenantTest(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}