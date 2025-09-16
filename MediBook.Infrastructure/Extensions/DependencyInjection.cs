using MediBook.Application.Interfaces;
using MediBook.Infrastructure.Data;
using MediBook.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediBook.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MediBookDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MediBook")));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, UserService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        return services;
    }
}
