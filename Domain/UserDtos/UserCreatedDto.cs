using Domain.DeviceInfoDtos;

namespace Domain.UserDtos;

public class UserCreatedDto
{
    public required UserDto User { get; init; }
    public required DeviceDto Device { get; init; }
}