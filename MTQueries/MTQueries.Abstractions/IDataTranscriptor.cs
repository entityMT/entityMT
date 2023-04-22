using System.Data;

namespace MTQueries.Abstractions;

public interface IDataTranscriptor<out T>
{
    IEnumerable<T> Transcript(DataTable source);
}