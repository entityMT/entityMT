using System.Reflection;
using MTPConfigurations.Abstractions.Enums;

namespace MTPConfigurations.Abstractions.Providers
{
    public interface IJoinTypeProvider<T>
    {
        JoinType GetJoinType(PropertyInfo propertyInfo);
    }
}