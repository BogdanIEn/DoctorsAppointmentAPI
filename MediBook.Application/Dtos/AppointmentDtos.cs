namespace MediBook.Application.Dtos;

public record AppointmentDto(
    int Id,
    int UserId,
    int DoctorId,
    string DoctorName,
    DateOnly Date,
    TimeOnly Time,
    string Reason,
    string Status,
    DateTime CreatedAt);

public record CreateAppointmentDto(
    int UserId,
    int DoctorId,
    string DoctorName,
    DateOnly Date,
    TimeOnly Time,
    string Reason,
    string Status = "confirmed");

public record UpdateAppointmentDto(
    int UserId,
    int DoctorId,
    string DoctorName,
    DateOnly Date,
    TimeOnly Time,
    string Reason,
    string Status);
