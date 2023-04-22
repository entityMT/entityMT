using System;

namespace MtTenants.Abstractions
{
    public interface ITenantFactory
    {
        ITenant Create(Guid id, string name);
    }
}