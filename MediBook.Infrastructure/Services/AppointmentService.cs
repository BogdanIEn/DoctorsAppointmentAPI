using MediBook.Application.Common.Mappings;
using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;
using MediBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Infrastructure.Services;

public class AppointmentService(MediBookDbContext context) : IAppointmentService
{
    private readonly MediBookDbContext _context = context;

    public async Task<IEnumerable<AppointmentDto>> GetAsync(int? userId, int? doctorId, CancellationToken cancellationToken = default)
    {
        IQueryable<Appointment> query = _context.Appointments.AsNoTracking();

        if (userId.HasValue)
        {
            query = query.Where(appointment => appointment.UserId == userId.Value);
        }

        if (doctorId.HasValue)
        {
            query = query.Where(appointment => appointment.DoctorId == doctorId.Value);
        }

        return await query
            .OrderByDescending(appointment => appointment.Date)
            .ThenByDescending(appointment => appointment.Time)
            .Select(appointment => appointment.ToDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments.AsNoTracking().FirstOrDefaultAsync(apt => apt.Id == id, cancellationToken);
        return appointment?.ToDto();
    }

    public async Task<AppointmentDto?> CreateAsync(CreateAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        if (!await _context.Users.AnyAsync(user => user.Id == dto.UserId, cancellationToken))
        {
            throw new ArgumentException("User not found", nameof(dto.UserId));
        }

        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == dto.DoctorId, cancellationToken);
        if (doctor is null)
        {
            throw new ArgumentException("Doctor not found", nameof(dto.DoctorId));
        }

        if (await HasConflict(dto.DoctorId, dto.Date, dto.Time, cancellationToken))
        {
            return null;
        }

        var appointment = new Appointment
        {
            UserId = dto.UserId,
            DoctorId = dto.DoctorId,
            DoctorName = doctor.Name,
            Date = dto.Date.ToDateTime(TimeOnly.MinValue),
            Time = dto.Time.ToTimeSpan(),
            Reason = dto.Reason,
            Status = dto.Status
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);
        return appointment.ToDto();
    }

    public async Task<AppointmentDto?> UpdateAsync(int id, UpdateAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(apt => apt.Id == id, cancellationToken);
        if (appointment is null)
        {
            return null;
        }

        if (!await _context.Users.AnyAsync(user => user.Id == dto.UserId, cancellationToken))
        {
            throw new ArgumentException("User not found", nameof(dto.UserId));
        }

        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == dto.DoctorId, cancellationToken);
        if (doctor is null)
        {
            throw new ArgumentException("Doctor not found", nameof(dto.DoctorId));
        }

        if (await HasConflict(dto.DoctorId, dto.Date, dto.Time, cancellationToken, id))
        {
            return null;
        }

        appointment.UserId = dto.UserId;
        appointment.DoctorId = dto.DoctorId;
        appointment.DoctorName = doctor.Name;
        appointment.Date = dto.Date.ToDateTime(TimeOnly.MinValue);
        appointment.Time = dto.Time.ToTimeSpan();
        appointment.Reason = dto.Reason;
        appointment.Status = dto.Status;

        await _context.SaveChangesAsync(cancellationToken);
        return appointment.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(apt => apt.Id == id, cancellationToken);
        if (appointment is null)
        {
            return false;
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<bool> HasConflict(int doctorId, DateOnly date, TimeOnly time, CancellationToken cancellationToken, int? ignoreId = null)
    {
        return await _context.Appointments.AnyAsync(appointment =>
            appointment.DoctorId == doctorId &&
            appointment.Date == date.ToDateTime(TimeOnly.MinValue) &&
            appointment.Time == time.ToTimeSpan() &&
            appointment.Status != "cancelled" &&
            appointment.Id != ignoreId,
            cancellationToken);
    }
}
