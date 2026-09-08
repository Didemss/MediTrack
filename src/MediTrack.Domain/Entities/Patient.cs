namespace MediTrack.Domain.Entities;

public class Patient : BaseEntity
{
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? BloodType { get; set; }

    public string? EmergencyContact { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}