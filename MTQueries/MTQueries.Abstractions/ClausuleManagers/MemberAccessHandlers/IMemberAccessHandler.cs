using System.Linq.Expressions;

namespace MTQueries.Abstractions.ClausuleManagers.MemberAccessHandlers
{
    public interface IMemberAccessHandler
    {
        bool Handle(MemberExpression node, out object value);
    }
}