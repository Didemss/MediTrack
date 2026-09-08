using MediTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PatientsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _context.Patients
            .AsNoTracking()
            .Include(x => x.User)
            .Select(x => new
            {
                x.Id,
                x.UserId,
                FullName = x.User.FirstName + " " + x.User.LastName,
                x.User.Email,
                x.DateOfBirth,
                x.PhoneNumber,
                x.BloodType
            })
            .ToListAsync();

        return Ok(patients);
    }
}