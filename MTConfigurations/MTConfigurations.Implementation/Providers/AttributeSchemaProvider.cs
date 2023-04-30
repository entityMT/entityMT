using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Providers;

namespace MTConfigurations.Implementation.Providers
{
    public sealed class AttributeSchemaProvider : ISchemaProvider
    {
        public string GetSchema(object entity)
        {
            var schemaAttributes = entity.GetType().GetCustomAttributes(typeof(SchemaAttribute), true);

            if (schemaAttributes == null || schemaAttributes.Length == 0)
                throw new ApplicationException("Schema attribute was not configurated.");

            return schemaAttributes[0].ToString()!;
        }
    }
}