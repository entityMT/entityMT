using System.Reflection;
using MTConfigurations.Abstractions.Enums;

namespace MTConfigurations.Abstractions.Providers
{
    public interface IJoinTypeProvider<T>
    {
        JoinType GetJoinType(PropertyInfo propertyInfo);
    }
}