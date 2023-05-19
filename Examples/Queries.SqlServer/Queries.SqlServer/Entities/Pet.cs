using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace Queries.SqlServer.Entities;

[Schema("dbo")]
[Table(nameof(Pet))]
public sealed class Pet
{
    public Pet()
    {
        MedicalHistory = new List<PetMedicalHistory>();
    }
    
    [Key]
    [Column("petId", ValueGenerated.Automatic)]
    public int Id { get; set; }

    [Column("petName", ValueGenerated.Manual)]
    public string Name { get; set; } = null!;
    
    [Column("years", ValueGenerated.Manual)]
    public int Years { get; set; }
    
    [Column("tutorId", ValueGenerated.Automatic)]
    [ForeignKey(typeof(Tutor))]
    [Join(JoinType.Inner)]
    public int TutorId { get; set; }
    
    public Tutor Tutor { get; set; } = null!;
    
    [Ghost]
    public IEnumerable<PetMedicalHistory> MedicalHistory { get; }
}