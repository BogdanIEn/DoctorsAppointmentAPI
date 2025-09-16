using MediBook.Application.Common.Mappings;
using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;
using MediBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Infrastructure.Services;

public class UserService(MediBookDbContext context) : IUserService, IAuthService
{
    private readonly MediBookDbContext _context = context;

    public async Task<IEnumerable<UserDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Select(user => user.ToDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        return user?.ToDto();
    }

    public async Task<UserDto?> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email, cancellationToken))
        {
            return null;
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Role = string.IsNullOrWhiteSpace(dto.Role) ? "patient" : dto.Role,
            Password = dto.Password
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user.ToDto();
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (user is null)
        {
            return null;
        }

        if (!string.Equals(user.Email, dto.Email, StringComparison.OrdinalIgnoreCase) &&
            await _context.Users.AnyAsync(u => u.Email == dto.Email, cancellationToken))
        {
            return null;
        }

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Phone = dto.Phone;
        user.Role = dto.Role;
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.Password = dto.Password!;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return user.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (user is null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<UserDto?> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password, cancellationToken);
        return user?.ToDto();
    }

    public async Task<UserDto?> RegisterAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        return await CreateAsync(dto, cancellationToken);
    }
}
