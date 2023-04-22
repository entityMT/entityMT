namespace MTQueries.Abstractions;

public interface IQuerySelectedColumnsProvider<T>
{
    Task<string[]> GetColumnsAsync(CancellationToken cancellationToken = default);
}