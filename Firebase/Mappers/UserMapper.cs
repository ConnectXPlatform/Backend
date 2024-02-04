using System.Collections.ObjectModel;
using Domain.UserDtos;
using Firebase.Entities;
using Firebase.Entities.User;

namespace Firebase.Mappers;

public static class UserMapper
{
    public static UserEntity ToEntity(this UserDto userDto)
    {
        return new UserEntity
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Devices = userDto.Devices,
            ControlPanels = userDto.ControlPanels,
        };
    }

    internal static UpdateUserEntity ToEntity(this UpdateUserDto updateDto)
    {
        return new UpdateUserEntity
        {
            Name = updateDto.Name
        };
    }
    
    internal static UserEntity ToEntity(this CreateUser user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Name = user.Name,
            ControlPanels = ReadOnlyCollection<string>.Empty,
            Devices = user.Devices
        };
    }

    public static UserDto ToDto(this UserEntity userEntity)
    {
        return new UserDto
        {
            Id = userEntity.Id,
            Name = userEntity.Name,
            Devices = userEntity.Devices,
            ControlPanels = userEntity.ControlPanels,
        };
    }
}