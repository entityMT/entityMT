using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MtPersistency.ConnectionManagers
{
    public interface IDbConnectionPersistencyManager : IDisposable
    {
        void Open();
        Task OpenAsync(CancellationToken cancellationToken);
        void Execute(string command, IDictionary<string, object> parameters);
        Task ExecuteAsync(string command, IDictionary<string, object> parameters, CancellationToken cancellationToken);
    }
}