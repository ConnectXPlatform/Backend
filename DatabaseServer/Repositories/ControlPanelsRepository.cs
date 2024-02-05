using Firebase;
using Domain.ControlPanelDtos;
using DatabaseServer.Endpoints;

namespace DatabaseServer.Repositories;

internal sealed class ControlPanelsRepository : IControlPanelsRepository
{
    private readonly FirebaseService firebaseService;

    public ControlPanelsRepository(FirebaseService firebaseService)
    {
        this.firebaseService = firebaseService;
    }

    public async Task<ControlPanelDto> AddControlPanel(CreateControlPanelDto controlPanel,
        CancellationToken cancellationToken)
    {
        return await firebaseService.AddControlPanel(controlPanel, cancellationToken);
    }

    public async Task<ControlPanelDto?> GetControlPanel(string controlPanelId, CancellationToken cancellationToken)
    {
        return await firebaseService.GetControlPanel(controlPanelId, cancellationToken);
    }

    public async Task UpdateControlPanel(string controlPanelId, UpdateControlPanelDto update,
        CancellationToken cancellationToken)
    {
        await firebaseService.UpdateControlPanel(controlPanelId, update, cancellationToken);
    }

    public async Task DeleteControlPanel(string controlPanelId, CancellationToken cancellationToken)
    {
        await firebaseService.DeleteControlPanel(controlPanelId, cancellationToken);
    }

    public async Task<IDictionary<string, ControlPanelDto?>> GetControlPanels(ISet<string> controlPanelIds,
        CancellationToken cancellationToken)
    {
        var panels = await firebaseService.GetControlPanelsForIds(controlPanelIds, cancellationToken);
        return controlPanelIds
             .ToDictionary(keySelector: id => id,
                 elementSelector: id => panels.TryGetValue(id, out var panel) ? panel : null);
    }
}