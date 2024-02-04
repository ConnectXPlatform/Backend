using System.Collections.Immutable;
using Firebase.Entities;
using Firebase.Mappers;
using Google.Cloud.Firestore;
using Domain.Components;

namespace Firebase.CollectionHandlers;

internal static class ComponentsCollection
{
    private const string CollectionId = "Components";

    public static async Task<ComponentDto?> GetComponent(this FirestoreDb db, string componentId, CancellationToken cancellationToken)
    {
        var snapshot = await db.Collection(CollectionId)
            .Document(componentId)
            .GetSnapshotAsync(cancellationToken);
        if (!snapshot.Exists)
            return null;

        var componentEntity = snapshot.ConvertTo<ComponentEntity>();
        return componentEntity.ToDto();
    }

    public static async Task<ComponentDto> AddComponent(this FirestoreDb db, CreateComponentDto componentDto, CancellationToken cancellationToken)
    {
        CollectionReference collection = db.Collection(CollectionId);
        DocumentReference document = collection.Document();
        ComponentEntity componentEntity = componentDto.ToEntity(document.Id);
        IDictionary<string, object> entityData = ObjectUtils.ToDictionary(componentEntity);
        await document.CreateAsync(entityData, cancellationToken);
        return componentEntity.ToDto();
    }

    public static async Task DeleteComponent(this FirestoreDb db, string componentId, CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(componentId);
        await document.DeleteAsync(cancellationToken: cancellationToken);
    }

    public static async Task<ICollection<ComponentDto>> GetAllComponents(this FirestoreDb db, CancellationToken cancellationToken)
    {
        var snapshot = await db.Collection(CollectionId)
            .GetSnapshotAsync(cancellationToken);
        if (snapshot.Count == 0)
            return ImmutableList<ComponentDto>.Empty;

        return snapshot.Documents
            .Select(document => document.ConvertTo<ComponentEntity>())
            .Select(entity => entity.ToDto())
            .ToImmutableList();
    }
}