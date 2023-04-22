using System;
using System.Linq.Expressions;

namespace MTQueries.Abstractions.ClausuleManagers
{
    public interface IOrderByClausuleManager<T>
    {
        void AddOrder<P>(Expression<Func<T, P>> exp, bool asc = true);
        
        void Reset();
        
        string GetOrderBy { get; }
    }
}