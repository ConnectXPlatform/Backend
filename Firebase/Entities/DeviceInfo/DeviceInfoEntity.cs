using Google.Cloud.Firestore;
using System.Collections.ObjectModel;

namespace Firebase.Entities.DeviceInfoEntities;

[FirestoreData]
public sealed class StatusEntity
{
    [FirestoreProperty("value")]
    public required int Value { get; init; }
    [FirestoreProperty("lastChange")]
    public required long LastChange { get; init; }
}

[FirestoreData]
public sealed class DeviceInfoEntity
{
    [FirestoreDocumentId]
    public required string Id { get; init; }
    [FirestoreProperty("name")]
    public required string Name { get; init; }
    [FirestoreProperty("description")]
    public string Description { get; init; } = string.Empty;
    [FirestoreProperty("status")]
    public required StatusEntity Status { get; init; }
    [FirestoreProperty("topics")]
    public IList<string> Topics { get; init; } = ReadOnlyCollection<string>.Empty;
}