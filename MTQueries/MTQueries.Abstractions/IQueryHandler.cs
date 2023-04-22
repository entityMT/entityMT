using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MTQueries.Abstractions
{
    public interface IQueryHandler<T> : IDisposable
    {
        IEnumerable<T> Handle(IQuery query);

        Task<IEnumerable<T>> HandleAsync(IQuery query, CancellationToken cancellationToken);
    }
}