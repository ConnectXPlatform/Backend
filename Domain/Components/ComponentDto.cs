namespace Domain.Components;

public sealed class ComponentDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required (int min, int max) WidthRange { get; init; }
    public required (int min, int max) HeightRange { get; init; }
}

public sealed class PositionedComponentDto
{
    public required string Id { get; init; }
    public required string ComponentId { get; init; }
    public required string TopicId { get; init; }
    public required string DeviceId { get; init; }
    public string Label { get; init; } = string.Empty;
    public required Size Size { get; init; }
    public required Position Position { get; init; }
}