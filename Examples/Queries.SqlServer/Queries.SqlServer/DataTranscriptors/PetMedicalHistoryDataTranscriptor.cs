using System.Data;
using MTQueries.Abstractions;
using Queries.SqlServer.Entities;

namespace Queries.SqlServer.DataTranscriptors;

public sealed class PetMedicalHistoryDataTranscriptor : IDataTranscriptor<PetMedicalHistory>
{
    public IEnumerable<PetMedicalHistory> Transcript(DataTable source)
    {
        var medicalHistories = new List<PetMedicalHistory>();
        var pets = new List<Pet>();
        var tutors = new List<Tutor>();

        foreach (DataRow sourceRow in source.Rows)
        {
            var medicalHistory = new PetMedicalHistory()
            {
                Id = Convert.ToInt32(sourceRow["petMedicalHistoryId"]),
                AppointmentDate = Convert.ToDateTime(sourceRow["appointmentDate"]),
                Comments = sourceRow["comments"].ToString()!
            };

            var pet = pets.FirstOrDefault(p => p.Id == Convert.ToInt32(sourceRow["petId"]));

            if (pet == default)
            {
                pet = new Pet()
                {
                    Id = Convert.ToInt32(sourceRow["petId"]),
                    Name = sourceRow["petName"].ToString()!,
                    Years = Convert.ToInt32(sourceRow["years"])
                };

                var tutor = tutors.FirstOrDefault(t => t.Id == Convert.ToInt32(sourceRow["tutorId"]));

                if (tutor == default)
                {
                    tutor = new Tutor()
                    {
                        Id = Convert.ToInt32(sourceRow["tutorId"]),
                        Name = sourceRow["tutorName"].ToString()!,
                        Document = sourceRow["document"].ToString()!
                    };
                    
                    tutors.Add(tutor);
                }

                pet.Tutor = tutor;
                (tutor.Pets as IList<Pet>)?.Add(pet);

                pets.Add(pet);
            }

            medicalHistory.Pet = pet;
            (pet.MedicalHistory as IList<PetMedicalHistory>)?.Add(medicalHistory);
            medicalHistories.Add(medicalHistory);
        }

        return medicalHistories;
    }
}