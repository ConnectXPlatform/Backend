using Domain.Components;
using Firebase.Entities;
using Firebase.Mappers;
using Google.Cloud.Firestore;

namespace Firebase.CollectionHandlers;

internal static class PositionedComponentsCollection
{
    private const string CollectionId = "PositionedComponents";

    public static async Task<PositionedComponentDto?> GetPositionedComponent(this FirestoreDb db, string componentId, CancellationToken cancellationToken)
    {
        var snapshot = await db.Collection(CollectionId)
            .Document(componentId)
            .GetSnapshotAsync(cancellationToken);
        if (!snapshot.Exists)
            return null;

        var componentEntity = snapshot.ConvertTo<PositionedComponentEntity>();
        return componentEntity.ToDto();
    }

    public static async Task<IEnumerable<PositionedComponentDto>> GetPositionedComponents(this FirestoreDb db, IEnumerable<string> ids,
        CancellationToken cancellationToken)
    {
        QuerySnapshot? snapshot = await db.Collection(CollectionId)
            .WhereIn(FieldPath.DocumentId, ids)
            .GetSnapshotAsync(cancellationToken);

        return snapshot.Documents
            .Select(document =>
            {
                var entity = document.ConvertTo<PositionedComponentEntity>();
                return entity.ToDto();
            });
    }

    public static async Task<PositionedComponentDto> AddPositionedComponent(this FirestoreDb db, CreatePositionedComponentDto componentDto, CancellationToken cancellationToken)
    {
        CollectionReference collection = db.Collection(CollectionId);
        DocumentReference document = collection.Document();
        PositionedComponentEntity componentEntity = componentDto.ToEntity(document.Id);
        IDictionary<string, object> entityData = ObjectUtils.ToDictionary(componentEntity);
        await document.CreateAsync(entityData, cancellationToken);
        return componentEntity.ToDto();
    }

    public static async Task UpdatePositionedComponent(this FirestoreDb db, string componentId,
        UpdatePositionedComponentDto update, CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(componentId);
        var updates = ObjectUtils.ToDictionary(update.ToEntity());
        await document.UpdateAsync(updates, cancellationToken: cancellationToken);
    }
    
    public static async Task DeletePositionedComponent(this FirestoreDb db, string componentId, CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(componentId);
        await document.DeleteAsync(cancellationToken: cancellationToken);
    }
}