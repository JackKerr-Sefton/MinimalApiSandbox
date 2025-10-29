using MinimalApiSandbox.Data;
using MinimalApiSandbox.Endpoints;
using MinimalApiSandbox.Services;

namespace MinimalApiSandbox;

public static class Bootstrapper
{
    public static void AddServices(this IServiceCollection services)
        => services
            // App Services
            .AddScoped<ICarparkService, CarparkService>()
            .AddScoped<ISchoolService, SchoolService>()

            // Data Services
            .AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            ;

    public static void AddEndpoints(this IEndpointRouteBuilder builder)
        => builder
            .AddCarparkEnpoints()
            .AddSchoolEnpoints()
            ;
}
