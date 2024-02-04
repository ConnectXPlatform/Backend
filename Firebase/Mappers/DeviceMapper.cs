using Domain.DeviceInfoDtos;
using Firebase.Entities.DeviceInfoEntities;

namespace Firebase.Mappers;

public static class DeviceMapper
{
    public static DeviceInfoEntity ToEntity(this DeviceDto deviceDto)
    {
        return new DeviceInfoEntity
        {
            Id = deviceDto.Id,
            Name = deviceDto.Name,
            Description = deviceDto.Description,
            Status = StatusDtoToEntity(deviceDto.Status),
            Topics = deviceDto.Topics
        };
    }

    internal static UpdateDeviceEntity ToEntity(this UpdateDeviceDto updateDto)
    {
        return new UpdateDeviceEntity
        {
            Name = updateDto.Name,
            Description = updateDto.Description,
            Status = updateDto.Status is not null
                ? StatusDtoToEntity(updateDto.Status)
                : null
        };
    }

    internal static DeviceInfoEntity ToEntity(this CreateDeviceDto device, string id)
    {
        return new DeviceInfoEntity
        {
            Id = id,
            Name = device.Name,
            Description = device.Description,
            Topics = device.Topics,
            Status = new StatusEntity
            {
                Value = (int)StatusEnum.Disconnected,
                LastChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            }
        };
    }

    internal static DeviceDto ToDto(this DeviceInfoEntity deviceInfoEntity)
    {
        return new DeviceDto
        {
            Id = deviceInfoEntity.Id,
            Name = deviceInfoEntity.Name,
            Description = deviceInfoEntity.Description,
            Status = StatusEntityToDto(deviceInfoEntity.Status),
            Topics = deviceInfoEntity.Topics
        };
    }

    private static StatusEntity StatusDtoToEntity(StatusDto statusDto)
    {
        return new StatusEntity
        {
            Value = (int)statusDto.Value,
            LastChange = statusDto.LastChange
        };
    }

    private static StatusDto StatusEntityToDto(StatusEntity statusEntity)
    {
        return new StatusDto
        {
            Value = (StatusEnum)statusEntity.Value,
            LastChange = statusEntity.LastChange
        };
    }
}