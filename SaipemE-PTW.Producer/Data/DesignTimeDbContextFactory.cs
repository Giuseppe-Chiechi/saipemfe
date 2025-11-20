using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SaipemE_PTW.Producer.Data
{
 public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
 {
 public AppDbContext CreateDbContext(string[] args)
 {
 var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
 // Connection string (Trusted Connection using Windows auth)
 var connectionString = "Server=localhost;Database=SaipemPWT;Trusted_Connection=True;TrustServerCertificate=True;";
 optionsBuilder.UseSqlServer(connectionString);
 return new AppDbContext(optionsBuilder.Options);
 }
 }
}
