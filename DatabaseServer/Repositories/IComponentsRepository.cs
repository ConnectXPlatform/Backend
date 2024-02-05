using Domain.Components;

namespace DatabaseServer.Repositories;

internal interface IComponentsRepository
{
    Task<ICollection<ComponentDto>> GetAllComponents(CancellationToken cancellationToken);
    Task<ComponentDto?> GetComponent(string componentId, CancellationToken cancellationToken);
    Task<ComponentDto> AddComponent(CreateComponentDto componentDto, CancellationToken cancellationToken);
    Task DeleteComponent(string componentId, CancellationToken cancellationToken);
}