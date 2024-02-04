using Google.Cloud.Firestore;

namespace Firebase.Entities.Topic;

[FirestoreData]
public sealed class TopicEntity
{
    [FirestoreDocumentId]
    public required string Id { get; init; }
    [FirestoreProperty("topic")]
    public required string Topic { get; init; }
    [FirestoreProperty("dataType")]
    public required string DataType { get; init; }
    [FirestoreProperty("modeFlags")]
    public required int ModeFlags { get; init; }
}