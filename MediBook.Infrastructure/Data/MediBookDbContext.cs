using MediBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Infrastructure.Data;

public class MediBookDbContext(DbContextOptions<MediBookDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(user => user.Role)
            .HasMaxLength(32);

        modelBuilder.Entity<Doctor>()
            .Property(doctor => doctor.Rating)
            .HasPrecision(3, 1);

        modelBuilder.Entity<Appointment>()
            .HasIndex(appointment => new { appointment.DoctorId, appointment.Date, appointment.Time })
            .IsUnique();

        modelBuilder.Entity<Appointment>()
            .Property(appointment => appointment.Status)
            .HasMaxLength(32);
    }
}
