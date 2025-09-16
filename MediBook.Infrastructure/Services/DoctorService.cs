using MediBook.Application.Common.Mappings;
using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;
using MediBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Infrastructure.Services;

public class DoctorService(MediBookDbContext context) : IDoctorService
{
    private readonly MediBookDbContext _context = context;

    public async Task<IEnumerable<DoctorDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
            .AsNoTracking()
            .Select(doctor => doctor.ToDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<DoctorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var doctor = await _context.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        return doctor?.ToDto();
    }

    public async Task<DoctorDto?> CreateAsync(CreateDoctorDto dto, CancellationToken cancellationToken = default)
    {
        var doctor = new Doctor
        {
            Name = dto.Name,
            Specialty = dto.Specialty,
            Experience = dto.Experience,
            Rating = dto.Rating
        };

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync(cancellationToken);
        return doctor.ToDto();
    }

    public async Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto dto, CancellationToken cancellationToken = default)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (doctor is null)
        {
            return null;
        }

        doctor.Name = dto.Name;
        doctor.Specialty = dto.Specialty;
        doctor.Experience = dto.Experience;
        doctor.Rating = dto.Rating;

        await _context.SaveChangesAsync(cancellationToken);
        return doctor.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (doctor is null)
        {
            return false;
        }

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
