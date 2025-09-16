using MediBook.Application.Dtos;
using MediBook.Domain.Entities;

namespace MediBook.Application.Common.Mappings;

public static class MappingExtensions
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
}
