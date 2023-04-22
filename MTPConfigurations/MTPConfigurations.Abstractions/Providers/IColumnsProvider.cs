using System.Collections.Generic;

namespace MTPConfigurations.Abstractions.Providers
{
    public interface IColumnsProvider
    {
        IEnumerable<Column> GetColumns(object entity);
    }
}