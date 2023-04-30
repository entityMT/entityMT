using System.Collections.Generic;

namespace MTConfigurations.Abstractions.Providers
{
    public interface IColumnsProvider
    {
        IEnumerable<Column> GetColumns(object entity);
    }
}