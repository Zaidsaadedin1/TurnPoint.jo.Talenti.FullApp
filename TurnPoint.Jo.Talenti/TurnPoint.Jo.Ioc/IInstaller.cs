using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TurnPoint.Jo.Ioc
{
    public interface IInstaller
    {

        int Order { get; }
        void AddServices(IServiceCollection services, IConfiguration configuration);

    }
}
