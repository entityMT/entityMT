using System;
using MtTenants.Abstractions;

namespace MtTenants.Implementation
{
    internal sealed class DefaultTenant : ITenant
    {
        public DefaultTenant(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}