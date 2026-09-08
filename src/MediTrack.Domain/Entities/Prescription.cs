namespace MediTrack.Domain.Entities;

public class Prescription : BaseEntity
{
    public Guid MedicalRecordId { get; set; }

    public MedicalRecord MedicalRecord { get; set; } = null!;

    public string MedicationName { get; set; } = string.Empty;

    public string Dosage { get; set; } = string.Empty;

    public string Frequency { get; set; } = string.Empty;

    public int DurationDays { get; set; }

    public string? Instructions { get; set; }
}