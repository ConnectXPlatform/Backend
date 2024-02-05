using Domain.ControlPanelDtos;

namespace DatabaseServer.Repositories;

internal interface IControlPanelsRepository
{
    Task<ControlPanelDto> AddControlPanel(CreateControlPanelDto controlPanel, CancellationToken cancellationToken);
    Task<ControlPanelDto?> GetControlPanel(string controlPanelId, CancellationToken cancellationToken);
    Task<IDictionary<string, ControlPanelDto?>> GetControlPanels(ISet<string> controlPanelIds, CancellationToken cancellationToken);
    Task UpdateControlPanel(string controlPanelId, UpdateControlPanelDto update, CancellationToken cancellationToken);
    Task DeleteControlPanel(string controlPanelId, CancellationToken cancellationToken);
}