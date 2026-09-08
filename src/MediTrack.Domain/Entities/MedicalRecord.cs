namespace MediTrack.Domain.Entities;

public class MedicalRecord : BaseEntity
{
    public Guid PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    public Guid DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;

    public Guid? AppointmentId { get; set; }

    public Appointment? Appointment { get; set; }

    public string Diagnosis { get; set; } = string.Empty;

    public string? Treatment { get; set; }

    public string? Notes { get; set; }

    public DateTime RecordDate { get; set; } = DateTime.UtcNow;

    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}