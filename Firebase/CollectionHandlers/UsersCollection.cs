using Domain.UserDtos;
using Firebase.Entities;
using Firebase.Entities.User;
using Firebase.Mappers;
using Google.Cloud.Firestore;

namespace Firebase.CollectionHandlers;

internal static class UsersCollection
{
    private const string CollectionId = "Users";

    public static async Task<UserDto?> GetUser(this FirestoreDb db, string userId, CancellationToken cancellationToken)
    {
        var snapshot = await db.Collection(CollectionId)
            .Document(userId)
            .GetSnapshotAsync(cancellationToken);
        if (!snapshot.Exists)
            return null;

        return snapshot.ConvertTo<UserEntity>().ToDto();
    }

    public static async Task<UserDto> AddUser(this FirestoreDb db, CreateUser user,
        CancellationToken cancellationToken)
    {
        CollectionReference collection = db.Collection(CollectionId);
        DocumentReference document = collection.Document(user.Id);
        UserEntity userEntity = user.ToEntity();
        IDictionary<string, object> entityData = ObjectUtils.ToDictionary(userEntity);
        await document.CreateAsync(entityData, cancellationToken);
        return userEntity.ToDto();
    }

    public static async Task UpdateUser(this FirestoreDb db, string userId, UpdateUserDto update,
        CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(userId);
        var updates = ObjectUtils.ToDictionary(update.ToEntity());
        var devicesUpdate = update.Devices.ToeUpdateObject();
        if (devicesUpdate is not null)
            updates["devices"] = devicesUpdate;
        var controlPanelsUpdate = update.ControlPanels.ToeUpdateObject();
        if (controlPanelsUpdate is not null)
            updates["controlPanels"] = controlPanelsUpdate;

        await document.UpdateAsync(updates, cancellationToken: cancellationToken);
    }

    public static async Task DeleteUser(this FirestoreDb db, string userId, CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(userId);
        await document.DeleteAsync(cancellationToken: cancellationToken);
    }

    public static async Task<bool> UserHasDevice(this FirestoreDb db, string userId, string deviceId)
    {
        var snapshot = await db.Collection(CollectionId)
            .WhereEqualTo(FieldPath.DocumentId, userId)
            .Where(
                Filter.Or(
                    Filter.ArrayContains("devices", deviceId),
                    Filter.EqualTo("intermediateId", deviceId)
                )
            )
            .Limit(1)
            .Count()
            .GetSnapshotAsync();

        return snapshot.Count > 0;
    }
}