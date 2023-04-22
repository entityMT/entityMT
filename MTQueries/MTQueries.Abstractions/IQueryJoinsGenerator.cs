namespace MTQueries.Abstractions;

public interface IQueryJoinsGenerator<T>
{
    Task<string[]> GetJoinsAsync(CancellationToken cancellationToken = default);
}