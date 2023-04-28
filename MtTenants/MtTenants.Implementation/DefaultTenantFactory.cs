using System;
using MtTenants.Abstractions;

namespace MtTenants.Implementation
{
    public sealed class DefaultTenantFactory : ITenantFactory
    {
        public ITenant Create(Guid id, string name)
        {
            return new DefaultTenant(id, name);
        }
    }
}