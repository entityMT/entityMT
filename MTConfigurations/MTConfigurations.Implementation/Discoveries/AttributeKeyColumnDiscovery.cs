using MTConfigurations.Abstractions;
using MTConfigurations.Abstractions.Discoveries;
using System.Reflection;
using MTConfigurations.Abstractions.Attributes;

namespace MTConfigurations.Implementation.Discoveries
{
    public sealed class AttributeKeyColumnDiscovery : IKeyColumnDiscovery
    {
        public Column Discovery(object entity)
        {
            var type = entity.GetType();
            
            var keyProperty = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
            
            if (keyProperty is null)
                throw new ApplicationException("Key was not configurated.");

            var configAttribute = keyProperty.GetCustomAttribute<ColumnAttribute>();

            if (configAttribute is null)
                throw new ApplicationException("Column configuration for key was not defined.");
            
            return new Column(keyProperty.Name, configAttribute.ValueGenerated);
        }
    }
}