using System.Collections.ObjectModel;

namespace Domain.DeviceInfoDtos;

public enum StatusEnum : byte
{
    Connected = 1 << 0,
    Disconnected = 1 << 1
}

public sealed class StatusDto
{
    public required StatusEnum Value { get; init; }
    public required long LastChange { get; init; }
}

public sealed class DeviceDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public required StatusDto Status { get; init; }
    public IList<string> Topics { get; init; } = ReadOnlyCollection<string>.Empty;
}