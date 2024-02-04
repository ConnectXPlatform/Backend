using Domain.DeviceInfoDtos;
using Firebase.Entities.DeviceInfoEntities;
using Firebase.Mappers;
using Google.Cloud.Firestore;

namespace Firebase.CollectionHandlers;

internal static class DevicesCollection
{
    private const string CollectionId = "Devices";

    public static async Task<DeviceDto?> GetDevice(this FirestoreDb db, string deviceId,
        CancellationToken cancellationToken)
    {
        var snapshot = await db.Collection(CollectionId)
            .Document(deviceId)
            .GetSnapshotAsync(cancellationToken);
        if (!snapshot.Exists)
            return null;

        var deviceEntity = snapshot.ConvertTo<DeviceInfoEntity>();
        return deviceEntity.ToDto();
    }

    public static async Task<IEnumerable<DeviceDto>> GetDevices(this FirestoreDb db, IEnumerable<string> ids,
        CancellationToken cancellationToken)
    {
        QuerySnapshot? snapshot = await db.Collection(CollectionId)
            .WhereIn(FieldPath.DocumentId, ids)
            .GetSnapshotAsync(cancellationToken);

        return snapshot.Documents
            .Select(document =>
            {
                var deviceEntity = document.ConvertTo<DeviceInfoEntity>();
                return deviceEntity.ToDto();
            });
    }

    public static DocumentReference NewDeviceDocument(this FirestoreDb db)
    {
        CollectionReference collection = db.Collection(CollectionId);
        DocumentReference document = collection.Document();
        return document;
    }

    public static async Task<DeviceDto> AddDevice(this FirestoreDb db, CreateDeviceDto device,
        CancellationToken cancellationToken)
    {
        CollectionReference collection = db.Collection(CollectionId);
        DocumentReference document = collection.Document();
        DeviceInfoEntity deviceInfoEntity = device.ToEntity(document.Id);
        IDictionary<string, object> entityData = ObjectUtils.ToDictionary(deviceInfoEntity);
        await document.CreateAsync(entityData, cancellationToken);
        return deviceInfoEntity.ToDto();
    }

    public static async Task UpdateDevice(this FirestoreDb db, string deviceId, UpdateDeviceDto update,
        CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(deviceId);
        var updates = ObjectUtils.ToDictionary(update.ToEntity());
        var topicsUpdate = update.Topics.ToeUpdateObject();
        if (topicsUpdate is not null)
            updates["topics"] = topicsUpdate;

        await document.UpdateAsync(updates, cancellationToken: cancellationToken);
    }

    public static async Task DeleteDevice(this FirestoreDb db, string deviceId, CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(deviceId);
        await document.DeleteAsync(cancellationToken: cancellationToken);
    }
}