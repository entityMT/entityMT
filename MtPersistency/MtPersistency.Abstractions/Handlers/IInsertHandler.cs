using System.Threading;
using System.Threading.Tasks;

namespace MtPersistency.Handlers
{
    public interface IInsertHandler
    {
        void Insert(object source);
        Task InsertAsync(object source, CancellationToken cancellationToken);
    }
}