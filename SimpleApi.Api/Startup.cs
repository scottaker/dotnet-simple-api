using SimpleApi.API.Infrastructure;

namespace SimpleApi.API;

public class Startup
{

    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        var dependency = new DependencyManager();
        dependency.Services(services, configuration);
    }

}
