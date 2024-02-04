using Firebase.Entities.ControlPanel;
using Domain.ControlPanelDtos;
using System.Collections.ObjectModel;

namespace Firebase.Mappers;

public static class ControlPanelMapper
{
    public static ControlPanelEntity ToEntity(this ControlPanelDto controlPanelDto)
    {
        return new ControlPanelEntity
        {
            Id = controlPanelDto.Id,
            Name = controlPanelDto.Name,
            Creator = controlPanelDto.Creator,
            Description = controlPanelDto.Description,
            Components = controlPanelDto.Components
        };
    }

    internal static UpdateControlPanelEntity ToEntity(this UpdateControlPanelDto updateDto)
    {
        return new UpdateControlPanelEntity
        {
            Name = updateDto.Name,
            Description = updateDto.Description
        };
    }

    internal static ControlPanelEntity ToEntity(this CreateControlPanelDto controlPanel, string id)
    {
        return new ControlPanelEntity
        {
            Id = id,
            Name = controlPanel.Name,
            Description = controlPanel.Description,
            Creator = controlPanel.Creator,
            Components = ReadOnlyCollection<string>.Empty
        };
    }

    public static ControlPanelDto ToDto(this ControlPanelEntity controlPanelEntity)
    {
        return new ControlPanelDto
        {
            Id = controlPanelEntity.Id,
            Name = controlPanelEntity.Name,
            Creator = controlPanelEntity.Creator,
            Description = controlPanelEntity.Description,
            Components = controlPanelEntity.Components
        };
    }
}