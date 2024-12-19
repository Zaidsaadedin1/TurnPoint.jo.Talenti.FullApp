using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace TurnPoint.Jo.Ioc
{
    public static class InstallerExtensions
    {
        public static void AddInstallers(this IServiceCollection services, Assembly assembly)
        {
            var installers = assembly.GetTypes()
                .Where(t => typeof(IInstaller).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .OrderBy(t => ((IInstaller)Activator.CreateInstance(t)).Order) 
                .ToList();

            foreach (var installer in installers)
            {
                var instance = (IInstaller)Activator.CreateInstance(installer);
                var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                instance.AddServices(services, configuration);
            }
        }
    }

}
