namespace Firebase.Entities.DeviceInfoEntities;

internal sealed class UpdateDeviceEntity
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public StatusEntity? Status { get; init; }
}