using Microsoft.AspNetCore.Mvc;
using MinimalApiSandbox.Data.Models;
using MinimalApiSandbox.Services;

namespace MinimalApiSandbox.Endpoints;

public static class SchoolEndpoints
{
    public static IEndpointRouteBuilder AddSchoolEnpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/schools", GetSchoolsAsync);
        builder.MapGet("/schools/{id}", GetSchoolByIdAsync);

        return builder;
    }

    private static async Task<IResult> GetSchoolsAsync([FromServices] ISchoolService service)
    {
        IEnumerable<School> carparks = await service.GetSchoolsAsync();

        return Results.Ok(carparks);
    }

    private static async Task<IResult> GetSchoolByIdAsync([FromRoute] int id, [FromServices] ISchoolService service)
    {
        School? carpark = await service.GetSchoolByIdAsync(id);

        return carpark is null
            ? Results.NotFound()
            : Results.Ok(carpark);
    }
}