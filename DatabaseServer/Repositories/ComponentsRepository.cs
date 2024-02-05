using Firebase;
using Domain.Components;

namespace DatabaseServer.Repositories;

internal sealed class ComponentsRepository : IComponentsRepository
{
    private readonly FirebaseService firebaseService;

    public ComponentsRepository(FirebaseService firebaseService)
    {
        this.firebaseService = firebaseService;
    }

    public async Task<ICollection<ComponentDto>> GetAllComponents(CancellationToken cancellationToken)
    {
        return await firebaseService.GetAllComponents(cancellationToken);
    }

    public async Task<ComponentDto?> GetComponent(string componentId, CancellationToken cancellationToken)
    {
        return await firebaseService.GetComponent(componentId, cancellationToken);
    }

    public async Task<ComponentDto> AddComponent(CreateComponentDto componentDto, CancellationToken cancellationToken)
    {
        return await firebaseService.AddComponent(componentDto, cancellationToken);
    }

    public async Task DeleteComponent(string componentId, CancellationToken cancellationToken)
    {
        await firebaseService.DeleteComponent(componentId, cancellationToken);
    }
}