using MediTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.PasswordHash)
                .IsRequired();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasIndex(x => x.Name)
                .IsUnique();

            entity.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(500);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasIndex(x => x.LicenseNumber)
                .IsUnique();

            entity.Property(x => x.LicenseNumber)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.ConsultationFee)
                .HasPrecision(10, 2);

            entity.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Doctor>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Department)
                .WithMany(x => x.Doctors)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Patient>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.PhoneNumber)
                .HasMaxLength(30);

            entity.Property(x => x.BloodType)
                .HasMaxLength(10);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasOne(x => x.Patient)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Doctor)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.Reason)
                .HasMaxLength(500);

            entity.Property(x => x.Notes)
                .HasMaxLength(1000);
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasOne(x => x.Patient)
                .WithMany(x => x.MedicalRecords)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Doctor)
                .WithMany()
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Appointment)
                .WithMany()
                .HasForeignKey(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Property(x => x.Diagnosis)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.Treatment)
                .HasMaxLength(1000);

            entity.Property(x => x.Notes)
                .HasMaxLength(2000);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasOne(x => x.MedicalRecord)
                .WithMany(x => x.Prescriptions)
                .HasForeignKey(x => x.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(x => x.MedicationName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Dosage)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Frequency)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Instructions)
                .HasMaxLength(1000);
        });
    }
}