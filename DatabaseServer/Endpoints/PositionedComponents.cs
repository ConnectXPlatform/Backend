using DatabaseServer.Repositories;
using Domain.Components;

namespace DatabaseServer.Endpoints;

internal static class PositionedComponents
{
    public static T MapPositionedComponentsEndpoints<T>(this T endpointRouteBuilder) where T : IEndpointRouteBuilder
    {
        endpointRouteBuilder.MapGet("/{componentId}", GetPositionedComponent)
            .Produces<PositionedComponentDto>()
            .WithName(nameof(GetPositionedComponent));
        endpointRouteBuilder.MapPost("/map", GetComponents)
            .Produces<IDictionary<string, PositionedComponentDto?>>();
        endpointRouteBuilder.MapPost("/", AddPositionedComponent)
            .Produces<ICollection<PositionedComponentDto>>(StatusCodes.Status201Created);
        endpointRouteBuilder.MapPut("/{componentId}", UpdatePositionedComponent);
        endpointRouteBuilder.MapDelete("/{componentId}", DeletePositionedComponent);
        return endpointRouteBuilder;
    }

    private static async Task<IResult> GetPositionedComponent(IPositionedComponentsRepository componentsRepository,
        string componentId, CancellationToken cancellationToken)
    {
        PositionedComponentDto? result =
            await componentsRepository.GetPositionedComponent(componentId, cancellationToken);
        if (result is null)
            return Results.NotFound();
        return Results.Ok(result);
    }

    private static async Task<IResult> GetComponents(IPositionedComponentsRepository componentsRepository, ISet<string> ids,
        CancellationToken cancellationToken)
    {
        return Results.Ok(await componentsRepository.GetPositionedComponents(ids, cancellationToken));
    }

    private static async Task<IResult> AddPositionedComponent(IPositionedComponentsRepository componentsRepository,
        CreatePositionedComponentDto componentDto, CancellationToken cancellationToken)
    {
        PositionedComponentDto component =
            await componentsRepository.AddPositionedComponent(componentDto, cancellationToken);
        return Results.CreatedAtRoute(nameof(GetPositionedComponent), new { componentId = component.Id }, component);
    }

    private static async Task<IResult> UpdatePositionedComponent(IPositionedComponentsRepository componentsRepository,
        string componentId, UpdatePositionedComponentDto update, CancellationToken cancellationToken)
    {
        await componentsRepository.UpdatePositionedComponent(componentId, update, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> DeletePositionedComponent(IPositionedComponentsRepository componentsRepository,
        string componentId, CancellationToken cancellationToken)
    {
        await componentsRepository.DeletePositionedComponent(componentId, cancellationToken);
        return Results.NoContent();
    }
}