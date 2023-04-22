using System;

namespace MtTenants.Abstractions
{
    public interface ITenant
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }
}