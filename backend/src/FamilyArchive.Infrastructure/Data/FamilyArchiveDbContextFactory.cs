using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FamilyArchive.Infrastructure.Data;

/// <summary>
/// Factory for creating FamilyArchiveDbContext at design time (e.g., for EF Core migrations).
/// </summary>
public class FamilyArchiveDbContextFactory : IDesignTimeDbContextFactory<FamilyArchiveDbContext>
{
    public FamilyArchiveDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FamilyArchive.Api"))
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true);

        if (environment == "Development")
        {
            builder.AddUserSecrets("a5c4e389-585d-4953-a957-25f76609ca79");
        }

        var configuration = builder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<FamilyArchiveDbContext>();
        var password = configuration["PGPASSWORD"];
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            .Replace("${PGPASSWORD}", password);

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        optionsBuilder.UseNpgsql(connectionString);

        return new FamilyArchiveDbContext(optionsBuilder.Options);
    }
}