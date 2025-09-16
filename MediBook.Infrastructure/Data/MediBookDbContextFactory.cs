using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MediBook.Infrastructure.Data;

public class MediBookDbContextFactory : IDesignTimeDbContextFactory<MediBookDbContext>
{
    private const string DefaultConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=MediBook;Trusted_Connection=True;MultipleActiveResultSets=true;";

    public MediBookDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MediBookDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("MEDIBOOK_CONNECTION_STRING") ?? DefaultConnectionString;
        optionsBuilder.UseSqlServer(connectionString);

        return new MediBookDbContext(optionsBuilder.Options);
    }
}
