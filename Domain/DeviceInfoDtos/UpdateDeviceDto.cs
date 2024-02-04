namespace Domain.DeviceInfoDtos;

public sealed class UpdateDeviceDto
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public StatusDto? Status { get; init; }
    public CollectionUpdateDto? Topics { get; init; }
}