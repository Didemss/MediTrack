using Microsoft.AspNetCore.Authorization;
using MediTrack.Domain.Entities;
using MediTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PrescriptionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var prescriptions = await _context.Prescriptions
            .AsNoTracking()
            .ToListAsync();

        return Ok(prescriptions);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePrescriptionRequest request)
    {
        var medicalRecordExists = await _context.MedicalRecords
            .AnyAsync(x => x.Id == request.MedicalRecordId);

        if (!medicalRecordExists)
        {
            return BadRequest("Medical record not found.");
        }

        var prescription = new Prescription
        {
            MedicalRecordId = request.MedicalRecordId,
            MedicationName = request.MedicationName.Trim(),
            Dosage = request.Dosage.Trim(),
            Frequency = request.Frequency.Trim(),
            Instructions = request.Instructions?.Trim()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        return Ok(prescription);
    }
}

public class CreatePrescriptionRequest
{
    public Guid MedicalRecordId { get; set; }

    public string MedicationName { get; set; } = string.Empty;

    public string Dosage { get; set; } = string.Empty;

    public string Frequency { get; set; } = string.Empty;

    public string? Instructions { get; set; }
}