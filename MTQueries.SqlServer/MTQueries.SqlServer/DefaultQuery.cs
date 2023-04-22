using MTQueries.Abstractions;

namespace MTQueries.SqlServer;

public sealed class DefaultQuery : IQuery
{
    public DefaultQuery(string content, IDictionary<string, object> parameters)
    {
        Content = content;
        Parameters = parameters;
    }
    
    public string Content { get; }
    public IDictionary<string, object> Parameters { get; }
}