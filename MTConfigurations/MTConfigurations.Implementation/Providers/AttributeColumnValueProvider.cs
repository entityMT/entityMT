using System.Reflection;
using MTConfigurations.Abstractions;
using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Providers;

namespace MTConfigurations.Implementation.Providers
{
    public sealed class AttributeColumnValueProvider
        : IColumnValueProvider
    {
        public object GetValue(object entity, Column column)
        {
            var properties = entity
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var property = properties.FirstOrDefault(prop =>
                prop.GetCustomAttributes<ColumnAttribute>()
                    .Any(
                        attr => 
                            attr.Name.Equals(column.Name)));

            if (property == null)
                throw new ApplicationException($"Column '{column.Name}' not bound to any property.");

            return property.GetValue(entity)!;
        }
    }
}