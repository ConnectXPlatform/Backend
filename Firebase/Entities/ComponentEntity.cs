using Google.Cloud.Firestore;

namespace Firebase.Entities;

[FirestoreData]
public sealed class ComponentEntity
{
    [FirestoreDocumentId]
    public required string Id { get; init; }
    [FirestoreProperty("name")]
    public required string Name { get; init; }
    [FirestoreProperty("minWidth")]
    public int MinWidth { get; init; }
    [FirestoreProperty("maxWidth")]
    public int MaxWidth { get; init; }
    [FirestoreProperty("minHeight")]
    public int MinHeight { get; init; }
    [FirestoreProperty("maxHeight")]
    public int MaxHeight { get; init; }
    [FirestoreProperty("maxHeight")]
    public int Mode { get; init; }
}

[FirestoreData]
public sealed class PositionedComponentEntity
{
    [FirestoreDocumentId]
    public required string Id { get; init; }
    [FirestoreProperty("componentId")]
    public required string ComponentId { get; init; }
    [FirestoreProperty("deviceId")]
    public required string DeviceId { get; init; }
    [FirestoreProperty("topicId")]
    public required string TopicId { get; init; }
    [FirestoreProperty("label")]
    public string Label { get; init; } = string.Empty;
    [FirestoreProperty("width")]
    public int Width { get; init; }
    [FirestoreProperty("height")]
    public int Height { get; init; }
    [FirestoreProperty("posX")]
    public int PosX { get; init; }
    [FirestoreProperty("posY")]
    public int PosY { get; init; }
}

public sealed class UpdatePositionedComponentEntity
{
    public string? TopicId { get; init; }
    public string? DeviceId { get; init; }
    public string? Label { get; init; }
    public int? Width { get; init; }
    public int? Height { get; init; }
    public int? PosX { get; init; }
    public int? PosY { get; init; }
}