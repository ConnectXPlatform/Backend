namespace Domain.UserDtos;

public sealed class NewUserDto
{
    public required string UserId { get; init; }
    public required string UserName { get; init; }
    public required string DeviceDescription { get; init; }
    public required string DeviceName { get; init; }
}