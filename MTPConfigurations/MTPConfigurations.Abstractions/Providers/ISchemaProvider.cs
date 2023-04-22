using System;

namespace MTPConfigurations.Abstractions.Providers
{
    public interface ISchemaProvider
    {
        string GetSchema(object entity);
    }
}