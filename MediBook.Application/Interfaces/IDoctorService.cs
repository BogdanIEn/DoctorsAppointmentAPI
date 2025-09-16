using MediBook.Application.Dtos;

namespace MediBook.Application.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAsync(CancellationToken cancellationToken = default);
    Task<DoctorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DoctorDto?> CreateAsync(CreateDoctorDto dto, CancellationToken cancellationToken = default);
    Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
