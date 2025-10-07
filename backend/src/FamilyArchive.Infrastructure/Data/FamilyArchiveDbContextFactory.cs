using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FamilyArchive.Infrastructure.Data;

public class FamilyArchiveDbContextFactory : IDesignTimeDbContextFactory<FamilyArchiveDbContext>
{
    public FamilyArchiveDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FamilyArchiveDbContext>();
        // Use your actual connection string here or read from environment/config
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=familyarchive;Username=youruser;Password=yourpassword");

        return new FamilyArchiveDbContext(optionsBuilder.Options);
    }
}