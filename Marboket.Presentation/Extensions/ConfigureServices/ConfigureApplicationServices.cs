using Marboket.Application;

namespace Marboket.Presentation.Extensions.ConfigureServices;

public static class ConfigureApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAutoMapper(ApplicationAssemblyReference.Assembly);

        return services;
    }
}
