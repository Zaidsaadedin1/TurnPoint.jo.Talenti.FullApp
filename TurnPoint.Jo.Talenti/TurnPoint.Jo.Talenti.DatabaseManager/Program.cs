using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TurnPoint.Jo.DatabaseManager.Data;


class Program
{
    static void Main(string[] args)
    {
        // Build the configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Configure DbContext with the connection string
        var serviceProvider = new ServiceCollection()
            .AddLogging(builder => builder.AddConsole())
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
            .BuildServiceProvider();

        var context = serviceProvider.GetService<ApplicationDbContext>();

        // Apply migrations to the database
        context.Database.Migrate();

        Console.WriteLine("Migrations applied successfully.");
    }
}
