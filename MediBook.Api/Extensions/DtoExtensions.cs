using MediBook.Api.Dtos;
using MediBook.Api.Models;

namespace MediBook.Api.Extensions;

public static class DtoExtensions
{
    public static UserDto ToDto(this User user) =>
        new(user.Id, user.Name, user.Email, user.Phone, user.Role, user.CreatedAt);

    public static DoctorDto ToDto(this Doctor doctor) =>
        new(doctor.Id, doctor.Name, doctor.Specialty, doctor.Experience, doctor.Rating);

    public static AppointmentDto ToDto(this Appointment appointment) =>
        new(
            appointment.Id,
            appointment.UserId,
            appointment.DoctorId,
            appointment.DoctorName,
            DateOnly.FromDateTime(appointment.Date),
            TimeOnly.FromTimeSpan(appointment.Time),
            appointment.Reason,
            appointment.Status,
            appointment.CreatedAt);

    public static void UpdateFromDto(this User user, UpdateUserDto dto)
    {
        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Phone = dto.Phone;
        user.Role = dto.Role;
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.Password = dto.Password!;
        }
    }

    public static void UpdateFromDto(this Doctor doctor, UpdateDoctorDto dto)
    {
        doctor.Name = dto.Name;
        doctor.Specialty = dto.Specialty;
        doctor.Experience = dto.Experience;
        doctor.Rating = dto.Rating;
    }

    public static void UpdateFromDto(this Appointment appointment, UpdateAppointmentDto dto)
    {
        appointment.UserId = dto.UserId;
        appointment.DoctorId = dto.DoctorId;
        appointment.DoctorName = dto.DoctorName;
        appointment.Date = dto.Date.ToDateTime(TimeOnly.MinValue);
        appointment.Time = dto.Time.ToTimeSpan();
        appointment.Reason = dto.Reason;
        appointment.Status = dto.Status;
    }
}
