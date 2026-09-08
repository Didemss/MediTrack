using Microsoft.AspNetCore.Authorization;
using MediTrack.Domain.Entities;
using MediTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace MediTrack.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MedicalRecordsController : ControllerBase
{
    private readonly AppDbContext _context;

    public MedicalRecordsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _context.MedicalRecords
            .AsNoTracking()
            .ToListAsync();

        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMedicalRecordRequest request)
    {
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == request.PatientId);

        if (!patientExists)
            return BadRequest("Patient not found.");

        var doctorExists = await _context.Doctors
            .AnyAsync(d => d.Id == request.DoctorId);

        if (!doctorExists)
            return BadRequest("Doctor not found.");

        if (request.AppointmentId.HasValue)
        {
            var appointmentExists = await _context.Appointments
                .AnyAsync(a => a.Id == request.AppointmentId.Value);

            if (!appointmentExists)
                return BadRequest("Appointment not found.");
        }

        var record = new MedicalRecord
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            AppointmentId = request.AppointmentId,
            Diagnosis = request.Diagnosis,
            Treatment = request.Treatment,
            Notes = request.Notes,
            RecordDate = DateTime.UtcNow
        };

        _context.MedicalRecords.Add(record);
        await _context.SaveChangesAsync();

        return Ok(record);
    }
}

public class CreateMedicalRecordRequest
{
    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public Guid? AppointmentId { get; set; }

    public string Diagnosis { get; set; } = string.Empty;

    public string? Treatment { get; set; }

    public string? Notes { get; set; }
}