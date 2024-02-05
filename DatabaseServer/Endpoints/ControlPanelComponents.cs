using DatabaseServer.Repositories;
using Domain;
using Domain.Components;
using Domain.ControlPanelDtos;

namespace DatabaseServer.Endpoints;

internal static class ControlPanelComponents
{
    public static T MapControlPanelComponentsEndpoints<T>(this T endpointRouteBuilder) where T : IEndpointRouteBuilder
    {
        endpointRouteBuilder.MapPost("/{controlPanelId}/components", AddComponent)
            .Produces<PositionedComponentDto>(StatusCodes.Status201Created);
        endpointRouteBuilder.MapDelete("/{controlPanelId}/components/{componentId}", DeleteTopic);
        return endpointRouteBuilder;
    }

    private static async Task<IResult> AddComponent(IPositionedComponentsRepository componentsRepository,
        IControlPanelsRepository controlPanelsRepository, string controlPanelId,
        CreatePositionedComponentDto componentDto, CancellationToken cancellationToken)
    {
        PositionedComponentDto created =
            await componentsRepository.AddPositionedComponent(componentDto, cancellationToken);
        await controlPanelsRepository.UpdateControlPanel(controlPanelId, new UpdateControlPanelDto
        {
            Components = new CollectionUpdateDto
            {
                Update = new HashSet<string> { created.Id },
                UpdateOperation = CollectionUpdateOperation.Add
            }
        }, cancellationToken);
        return Results.CreatedAtRoute("GetPositionedComponent", new { componentId = created.Id }, created);
    }

    private static async Task<IResult> DeleteTopic(IPositionedComponentsRepository componentsRepository,
        IControlPanelsRepository controlPanelsRepository, string controlPanelId, string componentId,
        CancellationToken cancellationToken)
    {
        await componentsRepository.DeletePositionedComponent(componentId, cancellationToken);
        await controlPanelsRepository.UpdateControlPanel(controlPanelId, new UpdateControlPanelDto
        {
            Components = new CollectionUpdateDto
            {
                Update = new HashSet<string> { componentId },
                UpdateOperation = CollectionUpdateOperation.Delete
            }
        }, cancellationToken);
        return Results.NoContent();
    }
}