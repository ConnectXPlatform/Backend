namespace Domain.DeviceInfoDtos;

public sealed class CreateDeviceDto
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public List<string> Topics { get; init; } = new(0);
}