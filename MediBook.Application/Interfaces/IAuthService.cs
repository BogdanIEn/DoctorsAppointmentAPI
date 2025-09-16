using MediBook.Application.Dtos;

namespace MediBook.Application.Interfaces;

public interface IAuthService
{
    Task<UserDto?> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);
    Task<UserDto?> RegisterAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
}
