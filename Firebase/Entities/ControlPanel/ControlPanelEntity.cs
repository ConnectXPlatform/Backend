using Google.Cloud.Firestore;
using System.Collections.ObjectModel;

namespace Firebase.Entities.ControlPanel;

[FirestoreData]
public sealed class ControlPanelEntity
{
    [FirestoreDocumentId]
    public required string Id { get; init; }
    [FirestoreProperty("name")]
    public required string Name { get; init; }
    [FirestoreProperty("creator")]
    public required string Creator { get; init; }
    [FirestoreProperty("description")]
    public string Description { get; init; } = string.Empty;
    [FirestoreProperty("components")]
    public IList<string> Components { get; init; } = ReadOnlyCollection<string>.Empty;
}