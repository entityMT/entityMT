using System;

namespace MTConfigurations.Abstractions.Providers
{
    public interface ISchemaProvider
    {
        string GetSchema(object entity);
    }
}