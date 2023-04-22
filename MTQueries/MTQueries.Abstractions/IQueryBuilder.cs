using System.Linq.Expressions;

namespace MTQueries.Abstractions
{
    public interface IQueryBuilder<T>
    {
        IQueryBuilder<T> SetGroup<P>(Expression<Func<T, P>> exp);
        IQueryBuilder<T> SetFilter(Expression<Func<T, bool>> exp);

        IQueryBuilder<T> SetOrder<P>(Expression<Func<T, P>> exp, bool asc = true);

        IQueryBuilder<T> SetDistinct();

        IQueryBuilder<T> SetCancellationToken(CancellationToken cancellationToken);

        IQuery Build();

        void Reset();
    }
}