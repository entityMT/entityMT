using System.Reflection;
using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;
using MTConfigurations.Abstractions.Providers;

namespace MTConfigurations.Implementation.Providers
{
    public sealed class AttributeJoinTypeProvider<T> : IJoinTypeProvider<T>
    {
        public JoinType GetJoinType(PropertyInfo propertyInfo)
        {
            var joinTypeAttribute = propertyInfo.GetCustomAttribute<JoinAttribute>(true);

            if (joinTypeAttribute == default)
                throw new ApplicationException($"Join type attribute was not configurated for property {propertyInfo.Name}");

            return joinTypeAttribute.JoinType;
        }
    }
}