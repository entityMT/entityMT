using MTConfigurations.Abstractions.Attributes;
using MTConfigurations.Abstractions.Enums;

namespace Queries.SqlServer.Entities;

[Schema("dbo")]
[Table(nameof(PetMedicalHistory))]
public sealed class PetMedicalHistory
{
    [Key]
    [Column("petMedicalHistoryId", ValueGenerated.Automatic)]
    public int Id { get; set; }
    
    [ForeignKey(typeof(Pet))]
    [Column("petId", ValueGenerated.Manual)]
    [Join(JoinType.Inner)]
    public int PetId { get; set; }
    
    public Pet Pet { get; set; } = null!;

    [Column("appointmentDate", ValueGenerated.Manual)]
    public DateTime AppointmentDate { get; set; }
    
    [Column("comments", ValueGenerated.Manual)]
    public string Comments { get; set; } = null!;
}