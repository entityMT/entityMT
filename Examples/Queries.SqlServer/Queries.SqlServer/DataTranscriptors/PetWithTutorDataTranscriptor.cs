using System.Data;
using MTQueries.Abstractions;
using Queries.SqlServer.Entities;

namespace Queries.SqlServer.DataTranscriptors;

public sealed class PetWithTutorDataTranscriptor : IDataTranscriptor<Pet>
{
    public IEnumerable<Pet> Transcript(DataTable source)
    {
        var pets = new List<Pet>();
        var tutors = new List<Tutor>();

        foreach (DataRow dataRow in source.Rows)
        {
            var pet = new Pet()
            {
                Id = Convert.ToInt32(dataRow["petId"]),
                Name = (dataRow["petName"].ToString() ?? null)!,
                Years = Convert.ToInt32(dataRow["years"])
            };

            var tutor = tutors.FirstOrDefault(t => t.Id == Convert.ToInt32(dataRow["tutorId"]));

            if (tutor != default)
                pet.Tutor = tutor;
            else
                tutor = new Tutor()
                {
                    Id = Convert.ToInt32(dataRow["tutorId"]),
                    Name = (dataRow["tutorName"].ToString() ?? null)!,
                    Document = (dataRow["document"].ToString() ?? null)!
                };
            
            (tutor.Pets as IList<Pet>)?.Add(pet);
            pet.Tutor = tutor;

            pets.Add(pet);
            tutors.Add(tutor);
        }
        
        return pets;
    }
}