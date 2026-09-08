using MediTrack.Domain.Entities;
using MediTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AppointmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _context.Appointments
            .AsNoTracking()
            .Include(x => x.Patient)
                .ThenInclude(x => x.User)
            .Include(x => x.Doctor)
                .ThenInclude(x => x.User)
            .Select(x => new
            {
                x.Id,
                x.PatientId,
                PatientName = x.Patient.User.FirstName + " " + x.Patient.User.LastName,
                x.DoctorId,
                DoctorName = x.Doctor.User.FirstName + " " + x.Doctor.User.LastName,
                x.AppointmentDate,
                x.DurationMinutes,
                x.Status,
                x.Reason,
                x.Notes
            })
            .OrderBy(x => x.AppointmentDate)
            .ToListAsync();

        return Ok(appointments);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var appointment = await _context.Appointments
            .AsNoTracking()
            .Include(x => x.Patient)
                .ThenInclude(x => x.User)
            .Include(x => x.Doctor)
                .ThenInclude(x => x.User)
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.PatientId,
                PatientName = x.Patient.User.FirstName + " " + x.Patient.User.LastName,
                x.DoctorId,
                DoctorName = x.Doctor.User.FirstName + " " + x.Doctor.User.LastName,
                x.AppointmentDate,
                x.DurationMinutes,
                x.Status,
                x.Reason,
                x.Notes
            })
            .FirstOrDefaultAsync();

        if (appointment is null)
        {
            return NotFound("Randevu bulunamadı.");
        }

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAppointmentRequest request)
    {
        if (request.AppointmentDate <= DateTime.Now)
        {
            return BadRequest("Geçmiş bir tarihe randevu oluşturulamaz.");
        }

        var patientExists = await _context.Patients
            .AnyAsync(x => x.Id == request.PatientId);

        if (!patientExists)
        {
            return BadRequest("Hasta bulunamadı.");
        }

        var doctorExists = await _context.Doctors
            .AnyAsync(x => x.Id == request.DoctorId);

        if (!doctorExists)
        {
            return BadRequest("Doktor bulunamadı.");
        }

        var conflictExists = await _context.Appointments
            .AnyAsync(x =>
                x.DoctorId == request.DoctorId &&
                x.AppointmentDate == request.AppointmentDate &&
                x.Status == AppointmentStatus.Scheduled);

        if (conflictExists)
        {
            return Conflict(
                "Bu doktorun aynı tarih ve saatte başka bir randevusu var.");
        }

        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            AppointmentDate = request.AppointmentDate,
            DurationMinutes = request.DurationMinutes <= 0
                ? 30
                : request.DurationMinutes,
            Status = AppointmentStatus.Scheduled,
            Reason = request.Reason?.Trim(),
            Notes = request.Notes?.Trim()
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = appointment.Id },
            new
            {
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.AppointmentDate,
                appointment.DurationMinutes,
                appointment.Status,
                appointment.Reason,
                appointment.Notes
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var appointment = await _context.Appointments
            .FindAsync(id);

        if (appointment is null)
        {
            return NotFound("Randevu bulunamadı.");
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public class CreateAppointmentRequest
{
    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public int DurationMinutes { get; set; } = 30;

    public string? Reason { get; set; }

    public string? Notes { get; set; }
}

  