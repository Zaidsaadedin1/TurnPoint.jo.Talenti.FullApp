using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TernPoint.Jo.Talenti.DatabaseManager
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Set the base path to the main application's directory (TurnPoint.Jo.APIs)
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "TurnPoint.Jo.APIs");  // Adjust the relative path accordingly

            // Build configuration from the main application's appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)  // Set the correct base path
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // Ensure appsettings.json exists in TurnPoint.Jo.APIs
                .Build();

            // Get the connection string from the configuration file
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Set up the DbContext options with the connection string
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
