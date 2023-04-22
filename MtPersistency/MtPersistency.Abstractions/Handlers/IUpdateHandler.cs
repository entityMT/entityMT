using System.Threading;
using System.Threading.Tasks;

namespace MtPersistency.Handlers
{
    public interface IUpdateHandler
    {
        void Update(object source);
        Task UpdateAsync(object source, CancellationToken cancellationToken);
    }
}