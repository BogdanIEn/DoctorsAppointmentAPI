namespace MediBook.Api.Dtos;

public record UserDto(int Id, string Name, string Email, string Phone, string Role, DateTime CreatedAt);

public record CreateUserDto(string Name, string Email, string Phone, string Role, string Password);

public record UpdateUserDto(string Name, string Email, string Phone, string Role, string? Password);

public record LoginRequestDto(string Email, string Password);

public record AuthResponseDto(UserDto User);
