namespace Domain.Components;

public sealed class UpdatePositionedComponentDto
{
    public string? TopicId { get; init; }
    public string? DeviceId { get; init; }
    public string? Label { get; init; }
    public Size? Size { get; init; }
    public Position? Position { get; init; }
}