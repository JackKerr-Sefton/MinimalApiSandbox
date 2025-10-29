using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSandbox.Endpoints;

public static class CarparkEndpoints
{
    public static IEndpointRouteBuilder AddCarparkEnpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/carparks", GetCarparks);
        builder.MapGet("/carparks/{id}", GetCarparkById);

        return builder;
    }

    private static IResult GetCarparks()
    {
        List<object> carparks = null;
        
        return Results.Ok(carparks);
    }

    private static IResult GetCarparkById([FromRoute] int id, [FromServices])
    {

        return Results.Ok();
    }
}
