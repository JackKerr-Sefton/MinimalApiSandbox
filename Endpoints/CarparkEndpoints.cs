using Microsoft.AspNetCore.Mvc;
using MinimalApiSandbox.Data.Models;
using MinimalApiSandbox.Services;

namespace MinimalApiSandbox.Endpoints;

public static class CarparkEndpoints
{
    public static IEndpointRouteBuilder AddCarparkEnpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/carparks", GetCarparksAsync);
        builder.MapGet("/carparks/{id}", GetCarparkByIdAsync);

        return builder;
    }

    private static async Task<IResult> GetCarparksAsync([FromServices] ICarparkService service)
    {
        IEnumerable<Carpark> carparks = await service.GetCarparksAsync();
        
        return Results.Ok(carparks);
    }

    private static async Task<IResult> GetCarparkByIdAsync([FromRoute] int id, [FromServices] ICarparkService service)
    {
        Carpark? carpark = await service.GetCarparkByIdAsync(id);

        return carpark is null
            ? Results.NotFound()
            : Results.Ok(carpark);
    }
}
