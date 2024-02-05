using Firebase;
using Domain.Components;
using DatabaseServer.Endpoints;

namespace DatabaseServer.Repositories;

internal sealed class PositionedComponentsRepository : IPositionedComponentsRepository
{
    private readonly FirebaseService firebaseService;

    public PositionedComponentsRepository(FirebaseService firebaseService)
    {
        this.firebaseService = firebaseService;
    }

    public async Task<PositionedComponentDto?> GetPositionedComponent(string componentId,
        CancellationToken cancellationToken)
    {
        return await firebaseService.GetPositionedComponent(componentId, cancellationToken);
    }

    public async Task<PositionedComponentDto> AddPositionedComponent(CreatePositionedComponentDto componentDto,
        CancellationToken cancellationToken)
    {
        return await firebaseService.AddPositionedComponent(componentDto, cancellationToken);
    }

    public async Task UpdatePositionedComponent(string componentId, UpdatePositionedComponentDto update,
        CancellationToken cancellationToken)
    {
        await firebaseService.UpdatePositionedComponent(componentId, update, cancellationToken);
    }

    public async Task DeletePositionedComponent(string componentId, CancellationToken cancellationToken)
    {
        await firebaseService.DeletePositionedComponent(componentId, cancellationToken);
    }

    public async Task<IDictionary<string, PositionedComponentDto?>> GetPositionedComponents(ISet<string> ids, CancellationToken cancellationToken)
    {
        var components = await firebaseService.GetPositionedComponents(ids, cancellationToken);
        return ids
             .ToDictionary(keySelector: id => id,
                 elementSelector: id => components.TryGetValue(id, out var component) ? component : null);
    }
}