using System.Threading;
using System.Threading.Tasks;

namespace MtPersistency.UnitOfWork
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        Task BeginTransactionAsync(CancellationToken cancellationToken);
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
    }
}