using System;
using System.Linq.Expressions;

namespace MTQueries.Abstractions.ClausuleManagers
{
    public interface IWhereClausuleManager<T>
    {
        void SetCondition(Expression<Func<T, bool>> exp);
        
        void Reset();
        
        IDictionary<string, object> Parameters { get; }
        
        string GetWhere { get; }
    }
}