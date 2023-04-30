using System.Reflection;
using MTConfigurations.Abstractions;
using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Providers;

namespace MTConfigurations.Implementation.Providers
{
    public sealed class AttributeColumnsProvider : IColumnsProvider
    {
        public IEnumerable<Column> GetColumns(object entity)
        {
            var properties = entity
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var columns = new List<Column>();

            foreach (var propertyInfo in properties)
            {
                var attribute = propertyInfo.GetCustomAttribute<ColumnAttribute>();

                if (attribute is not null)
                {
                    columns.Add(new Column(attribute.Name, attribute.ValueGenerated));
                }
            }
            
            if (!columns.Any())
                throw new ApplicationException("Table doesn't have any columns configurated");
            
            return columns;
        }
    }
}