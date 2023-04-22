using System;
using System.Linq.Expressions;

namespace MTQueries.Abstractions.ClausuleManagers
{
    public interface IGroupByClausuleManager<T>
    {
        void AddGroup<P>(Expression<Func<T, P>> exp);

        void Reset();
        
        string GetGroupBy { get; }
    }
}