using System.Linq.Expressions;
using System.Reflection;
using MTQueries.Abstractions.ClausuleManagers.MemberAccessHandlers;

namespace MTQueries.SqlServer.MemberAccessHandlers
{
    public sealed class PropertyInfoMemberAccessHandler : IMemberAccessHandler
    {
        public bool Handle(MemberExpression node, out object value)
        {
            if (node.Member.MemberType == MemberTypes.Property)
            {
                var property = node.Member as PropertyInfo;
                var propertyType = property?.GetType();
                
                if (propertyType != default && 
                    (propertyType.IsPrimitive
                        || propertyType == typeof(string)
                        || propertyType == typeof(decimal)
                        || propertyType == typeof(Guid)
                        || propertyType == typeof(DateTime)))
                {
                    var @delegate = Expression.Lambda<Func<object>>(node).Compile();
                    value = @delegate.DynamicInvoke()!;

                    return true;
                }
            }

            value = default!;
            return false;
        }
    }
}