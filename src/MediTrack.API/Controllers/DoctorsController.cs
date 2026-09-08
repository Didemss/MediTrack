using MediTrack.Domain.Entities;
using MediTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DoctorsController(AppDbContext context)
    {
        _context = context;
    }

    // Tüm doktorları getir
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _context.Doctors
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Department)
            .Select(x => new
            {
                x.Id,
                x.UserId,
                FullName = x.User.FirstName + " " + x.User.LastName,
                x.User.Email,
                x.LicenseNumber,
                x.DepartmentId,
                Department = x.Department.Name,
                x.ConsultationFee,
                x.Biography
            })
            .ToListAsync();

        return Ok(doctors);
    }

    // Test amaçlı doktor + bölüm oluştur
    [HttpPost("seed-test-doctor")]
    public async Task<IActionResult> SeedTestDoctor()
    {
        var existingDepartment = await _context.Departments
            .FirstOrDefaultAsync(x => x.Name == "Kardiyoloji");

        if (existingDepartment is null)
        {
            existingDepartment = new Department
            {
                Name = "Kardiyoloji",
                Description = "Kalp ve dolaşım sistemi hastalıkları"
            };

            _context.Departments.Add(existingDepartment);
            await _context.SaveChangesAsync();
        }

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == "doctor@meditrack.com");

        if (existingUser is not null)
        {
            var existingDoctor = await _context.Doctors
                .FirstOrDefaultAsync(x => x.UserId == existingUser.Id);

            if (existingDoctor is not null)
            {
                return Ok(new
                {
                    message = "Test doktoru zaten mevcut.",
                    doctorId = existingDoctor.Id
                });
            }
        }

        var doctorUser = new User
        {
            FirstName = "Ayşe",
            LastName = "Yılmaz",
            Email = "doctor@meditrack.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Doctor123!"),
            Role = UserRole.Doctor,
            IsActive = true
        };

        _context.Users.Add(doctorUser);
        await _context.SaveChangesAsync();

        var doctor = new Doctor
        {
            UserId = doctorUser.Id,
            DepartmentId = existingDepartment.Id,
            LicenseNumber = "MED-TEST-001",
            Biography = "Kardiyoloji uzmanı test doktoru.",
            ConsultationFee = 1000
        };

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Test doktoru oluşturuldu.",
            doctorId = doctor.Id,
            departmentId = existingDepartment.Id,
            email = doctorUser.Email
        });
    }
}