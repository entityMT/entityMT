using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Providers;

namespace MTConfigurations.Implementation.Providers
{
    public sealed class AttributeTableNameProvider : ITableNameProvider
    {
        public string GetTableName(object entity)
        {
            var tableAttributes = entity.GetType().GetCustomAttributes(typeof(TableAttribute), true);

            if (tableAttributes == null || tableAttributes.Length == 0)
                throw new ApplicationException("Table name attribute was not configurated");

            return tableAttributes[0].ToString()!;
        }
    }
}