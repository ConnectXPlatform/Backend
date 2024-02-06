namespace Domain.Components;

[Flags]
public enum ComponentModes : byte
{
    DataViewer = 1 << 0,
    CommandsSender = 1 << 1,
    Both = DataViewer | CommandsSender
}

public sealed class ComponentDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required (int min, int max) WidthRange { get; init; }
    public required (int min, int max) HeightRange { get; init; }
    public required ComponentModes Mode { get; init; }
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