using DatabaseServer.Repositories;
using Domain.Components;
using System.ComponentModel;

namespace DatabaseServer.Endpoints;

public static class Components
{
    public static T MapComponentsEndpoints<T>(this T endpointRouteBuilder) where T : IEndpointRouteBuilder
    {
        endpointRouteBuilder.MapGet("/{componentId}", GetComponent)
            .Produces<ComponentDto>()
            .WithName(nameof(GetComponent));
        endpointRouteBuilder.MapGet("/", GetAllComponents)
            .Produces<ICollection<ComponentDto>>();
        endpointRouteBuilder.MapPost("/", AddComponent)
            .Produces<ComponentDto>(StatusCodes.Status201Created);
        endpointRouteBuilder.MapDelete("/{componentId}", DeleteComponent);
        return endpointRouteBuilder;
    }

    private static async Task<IResult> GetComponent(IComponentsRepository componentsRepository, string componentId,
        CancellationToken cancellationToken)
    {
        ComponentDto? result = await componentsRepository.GetComponent(componentId, cancellationToken);
        if (result is null)
            return Results.NotFound();
        return Results.Ok(result);
    }

    private static async Task<IResult> GetAllComponents(IComponentsRepository componentsRepository,
        CancellationToken cancellationToken)
    {
        return Results.Ok(await componentsRepository.GetAllComponents(cancellationToken));
    }

    private static async Task<IResult> AddComponent(IComponentsRepository componentsRepository,
        CreateComponentDto componentDto, CancellationToken cancellationToken)
    {
        ComponentDto component = await componentsRepository.AddComponent(componentDto, cancellationToken);
        return Results.CreatedAtRoute(nameof(GetComponent), new { componentId = component.Id }, component);
    }

    private static async Task<IResult> DeleteComponent(IComponentsRepository componentsRepository, string componentId,
        CancellationToken cancellationToken)
    {
        await componentsRepository.DeleteComponent(componentId, cancellationToken);
        return Results.NoContent();
    }
}