using DatabaseServer.Repositories;
using Domain.ControlPanelDtos;

namespace DatabaseServer.Endpoints;

internal static class ControlPanels
{
    public static T MapControlPanelsEndpoints<T>(this T endpointRouteBuilder) where T : IEndpointRouteBuilder
    {
        endpointRouteBuilder.MapGet("/{controlPanelId}", GetControlPanel)
            .Produces<ControlPanelDto>()
            .WithName(nameof(GetControlPanel));
        endpointRouteBuilder.MapPost("/map", GetControlPanels)
            .Produces<IDictionary<string, ControlPanelDto>>();
        endpointRouteBuilder.MapPost("/", AddControlPanel)
            .Produces<ControlPanelDto>(StatusCodes.Status201Created);
        endpointRouteBuilder.MapPut("/{controlPanelId}", UpdateControlPanel);
        endpointRouteBuilder.MapDelete("/{controlPanelId}", DeleteControlPanel);
        return endpointRouteBuilder;
    }

    private static async Task<IResult> GetControlPanel(IControlPanelsRepository controlPanelsRepository,
        string controlPanelId, CancellationToken cancellationToken)
    {
        ControlPanelDto? panel = await controlPanelsRepository.GetControlPanel(controlPanelId, cancellationToken);
        if (panel is null)
            return Results.NotFound();
        return Results.Ok(panel);
    }

    private static async Task<IResult> GetControlPanels(IControlPanelsRepository controlPanelsRepository,
        ISet<string> controlPanelIds, CancellationToken cancellationToken)
    {
        return Results.Ok(await controlPanelsRepository.GetControlPanels(controlPanelIds, cancellationToken));
    }

    private static async Task<IResult> AddControlPanel(IControlPanelsRepository controlPanelsRepository,
        CreateControlPanelDto controlPanelDto, CancellationToken cancellationToken)
    {
        ControlPanelDto created = await controlPanelsRepository.AddControlPanel(controlPanelDto, cancellationToken);
        return Results.CreatedAtRoute(nameof(GetControlPanel), new { controlPanelId = created.Id }, created);
    }

    private static async Task<IResult> UpdateControlPanel(IControlPanelsRepository controlPanelsRepository,
        string controlPanelId, UpdateControlPanelDto update, CancellationToken cancellationToken)
    {
        await controlPanelsRepository.UpdateControlPanel(controlPanelId, update, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteControlPanel(IControlPanelsRepository controlPanelsRepository,
        string controlPanelId, CancellationToken cancellationToken)
    {
        await controlPanelsRepository.DeleteControlPanel(controlPanelId, cancellationToken);
        return Results.NoContent();
    }
}