using System.Data;
using MTQueries.Abstractions;
using Queries.SqlServer.Entities;

namespace Queries.SqlServer.DataTranscriptors;

public sealed class TutorOnlyDataTranscriptor : IDataTranscriptor<Tutor>
{
    public IEnumerable<Tutor> Transcript(DataTable source)
    {
        var tutors = new List<Tutor>();

        foreach (DataRow dataRow in source.Rows)
        {
            tutors.Add(new Tutor()
            {
                Id = Convert.ToInt32(dataRow["tutorId"]),
                Name = (dataRow["tutorName"].ToString() ?? null)!,
                Document = (dataRow["document"].ToString() ?? null)!
            });
        }

        return tutors;
    }
}