using Google.Cloud.Firestore;
using System.Collections.ObjectModel;

namespace Firebase.Entities.User;

[FirestoreData]
public sealed class UserEntity
{
    [FirestoreDocumentId]
    public required string Id { get; init; }
    [FirestoreProperty("name")]
    public required string Name { get; init; }
    [FirestoreProperty("devices")]
    public IList<string> Devices { get; init; } = ReadOnlyCollection<string>.Empty;
    [FirestoreProperty("controlPanels")]
    public IList<string> ControlPanels { get; init; } = ReadOnlyCollection<string>.Empty;
}