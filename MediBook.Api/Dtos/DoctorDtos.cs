namespace MediBook.Api.Dtos;

public record DoctorDto(int Id, string Name, string Specialty, string Experience, double Rating);

public record CreateDoctorDto(string Name, string Specialty, string Experience, double Rating);

public record UpdateDoctorDto(string Name, string Specialty, string Experience, double Rating);
