using System.Collections.Immutable;
using Domain.ControlPanelDtos;
using Firebase.Entities.ControlPanel;
using Firebase.Mappers;
using Google.Cloud.Firestore;

namespace Firebase.CollectionHandlers;

internal static class ControlPanelsCollection
{
    private const string CollectionId = "ControlPanels";

    public static async Task<IReadOnlySet<string>> GetComponents(this FirestoreDb db, string controlPanelId, CancellationToken cancellationToken)
    {
        ControlPanelDto? controlPanel = await GetControlPanel(db, controlPanelId, cancellationToken);
        if (controlPanel is null)
        {
            return ImmutableHashSet<string>.Empty;
        }
        return new HashSet<string>(controlPanel.Components);
    }

    public static async Task<ControlPanelDto> AddControlPanel(this FirestoreDb db, CreateControlPanelDto controlPanel,
        CancellationToken cancellationToken)
    {
        CollectionReference collection = db.Collection(CollectionId);
        DocumentReference document = collection.Document();
        ControlPanelEntity controlPanelInfoEntity = controlPanel.ToEntity(document.Id);
        IDictionary<string, object> entityData = ObjectUtils.ToDictionary(controlPanelInfoEntity);
        await document.CreateAsync(entityData, cancellationToken);
        return controlPanelInfoEntity.ToDto();
    }

    public static async Task<ControlPanelDto?> GetControlPanel(this FirestoreDb db, string controlPanelId,
        CancellationToken cancellationToken)
    {
        var snapshot = await db.Collection(CollectionId)
            .Document(controlPanelId)
            .GetSnapshotAsync(cancellationToken);
        if (!snapshot.Exists)
            return null;
 
        return snapshot.ConvertTo<ControlPanelEntity>().ToDto();
    }

    public static async Task<IEnumerable<ControlPanelDto>> GetControlPanels(this FirestoreDb db, IEnumerable<string> ids,
        CancellationToken cancellationToken)
    {
        QuerySnapshot? snapshot = await db.Collection(CollectionId)
            .WhereIn(FieldPath.DocumentId, ids)
            .GetSnapshotAsync(cancellationToken);

        return snapshot.Documents
            .Select(document =>
            {
                var entity = document.ConvertTo<ControlPanelEntity>();
                return entity.ToDto();
            });
    }

    public static async Task UpdateControlPanel(this FirestoreDb db, string controlPanelId, UpdateControlPanelDto update,
        CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(controlPanelId);
        var updates = ObjectUtils.ToDictionary(update.ToEntity());
        var componentsUpdate = update.Components.ToeUpdateObject();
        if (componentsUpdate is not null)
            updates["components"] = componentsUpdate;
    
        await document.UpdateAsync(updates, cancellationToken: cancellationToken);
    }

    public static async Task DeleteControlPanel(this FirestoreDb db, string controlPanelId, CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(controlPanelId);
        await document.DeleteAsync(cancellationToken: cancellationToken);
    }
}