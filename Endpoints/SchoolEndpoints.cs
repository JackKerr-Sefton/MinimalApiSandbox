using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSandbox.Endpoints;

public static class SchoolEndpoints
{
    public static IEndpointRouteBuilder AddSchoolEnpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/schools", GetSchools);
        builder.MapGet("/schools/{id}", GetSchoolById);

        return builder;
    }

    private static IResult GetSchools()
    {
        List<object> schools = null;

        return Results.Ok(schools);
    }

    private static IResult GetSchoolById([FromRoute] int id)
    {

        return Results.Ok();
    }
}