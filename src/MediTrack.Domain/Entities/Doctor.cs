namespace MediTrack.Domain.Entities;

public class Doctor : BaseEntity
{
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public Guid DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public string LicenseNumber { get; set; } = string.Empty;

    public string? Biography { get; set; }

    public decimal ConsultationFee { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}