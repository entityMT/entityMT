using System;
using System.Reflection;
using MTPConfigurations.Abstractions.Attributes;
using MTPConfigurations.Abstractions.Enums;
using MTPConfigurations.Abstractions.Providers;

namespace MTPConfigurations.Implementation.Providers
{
    internal sealed class AttributeJoinTypeProvider<T> : IJoinTypeProvider<T>
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