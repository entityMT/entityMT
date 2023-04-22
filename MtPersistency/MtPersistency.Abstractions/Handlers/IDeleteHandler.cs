using System.Threading;
using System.Threading.Tasks;

namespace MtPersistency.Handlers
{
    public interface IDeleteHandler
    {
        void Delete(object source);
        Task DeleteAsync(object source, CancellationToken cancellationToken);
    }
}