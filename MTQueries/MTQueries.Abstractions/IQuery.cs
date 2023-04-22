using System.Collections.Generic;

namespace MTQueries.Abstractions
{
    public interface IQuery
    {
        string Content { get; }
        IDictionary<string, object> Parameters { get; }
    }
}