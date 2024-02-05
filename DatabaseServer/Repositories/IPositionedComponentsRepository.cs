using Domain.Components;

namespace DatabaseServer.Repositories;

internal interface IPositionedComponentsRepository
{
    Task<PositionedComponentDto?> GetPositionedComponent(string componentId, CancellationToken cancellationToken);

    Task<PositionedComponentDto> AddPositionedComponent(CreatePositionedComponentDto componentDto,
        CancellationToken cancellationToken);

    Task UpdatePositionedComponent(string componentId, UpdatePositionedComponentDto update,
        CancellationToken cancellationToken);

    Task DeletePositionedComponent(string componentId, CancellationToken cancellationToken);
    Task<IDictionary<string, PositionedComponentDto?>> GetPositionedComponents(ISet<string> ids,
        CancellationToken cancellationToken);
}