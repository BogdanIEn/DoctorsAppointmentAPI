using MediBook.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Api.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(MediBookDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync())
        {
            context.Users.AddRange(
                new User
                {
                    Name = "Demo Admin",
                    Email = "admin@demo.com",
                    Password = "password123",
                    Phone = "+1234567890",
                    Role = "admin"
                },
                new User
                {
                    Name = "Demo Patient",
                    Email = "patient@demo.com",
                    Password = "password123",
                    Phone = "+1987654321",
                    Role = "patient"
                });
        }

        if (!await context.Doctors.AnyAsync())
        {
            context.Doctors.AddRange(
                new Doctor { Name = "Dr. Sarah Wilson", Specialty = "Cardiology", Experience = "15 years", Rating = 4.8 },
                new Doctor { Name = "Dr. Michael Chen", Specialty = "Dermatology", Experience = "12 years", Rating = 4.7 },
                new Doctor { Name = "Dr. Emily Johnson", Specialty = "Pediatrics", Experience = "18 years", Rating = 4.9 },
                new Doctor { Name = "Dr. David Rodriguez", Specialty = "Orthopedics", Experience = "20 years", Rating = 4.6 },
                new Doctor { Name = "Dr. Lisa Anderson", Specialty = "Neurology", Experience = "14 years", Rating = 4.8 },
                new Doctor { Name = "Dr. James Thompson", Specialty = "Internal Medicine", Experience = "16 years", Rating = 4.7 }
            );
        }

        await context.SaveChangesAsync();

        if (!await context.Appointments.AnyAsync())
        {
            var patient = await context.Users.FirstAsync(u => u.Email == "patient@demo.com");
            var doctor = await context.Doctors.FirstAsync(d => d.Name == "Dr. Sarah Wilson");

            context.Appointments.Add(new Appointment
            {
                UserId = patient.Id,
                DoctorId = doctor.Id,
                DoctorName = doctor.Name,
                Date = DateTime.UtcNow.Date.AddDays(3),
                Time = new TimeSpan(9, 0, 0),
                Reason = "Annual check-up",
                Status = "confirmed"
            });
        }

        await context.SaveChangesAsync();
    }
}
