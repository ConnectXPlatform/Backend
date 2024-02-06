using Domain.Components;
using Firebase.Entities;

namespace Firebase.Mappers;

internal static class ComponentMapper
{
    public static ComponentEntity ToEntity(this ComponentDto componentDto)
    {
        return new ComponentEntity
        {
            Id = componentDto.Id,
            Name = componentDto.Name,
            MinWidth = componentDto.WidthRange.min,
            MaxWidth = componentDto.WidthRange.max,
            MinHeight = componentDto.HeightRange.min,
            MaxHeight = componentDto.HeightRange.max,
            Mode = (int)componentDto.Mode
        };
    }

    public static ComponentEntity ToEntity(this CreateComponentDto componentDto, string componentId)
    {
        return new ComponentEntity
        {
            Id = componentId,
            Name = componentDto.Name,
            MinWidth = componentDto.WidthRange.min,
            MaxWidth = componentDto.WidthRange.max,
            MinHeight = componentDto.HeightRange.min,
            MaxHeight = componentDto.HeightRange.max,
            Mode = (int)componentDto.Mode
        };
    }

    public static ComponentDto ToDto(this ComponentEntity componentEntity)
    {
        (int min, int max) heightRange = (componentEntity.MinHeight, componentEntity.MaxHeight);
        (int min, int max) widthRange = (componentEntity.MinWidth, componentEntity.MaxWidth);
        return new ComponentDto
        {
            Id = componentEntity.Id,
            Name = componentEntity.Name,
            WidthRange = widthRange,
            HeightRange = heightRange,
            Mode = (ComponentModes)componentEntity.Mode
        };
    }
}

internal static class PositionedComponentMapper
{
    public static PositionedComponentEntity ToEntity(this PositionedComponentDto componentDto)
    {
        return new PositionedComponentEntity
        {
            Id = componentDto.Id,
            ComponentId = componentDto.Id,
            TopicId = componentDto.TopicId,
            DeviceId = componentDto.DeviceId,
            Label = componentDto.Label,
            Width = componentDto.Size.Width,
            Height = componentDto.Size.Height,
            PosX = (int) componentDto.Position.X,
            PosY = (int) componentDto.Position.Y
        };
    }

    public static PositionedComponentEntity ToEntity(this CreatePositionedComponentDto componentDto, string componentId)
    {
        return new PositionedComponentEntity
        {
            Id = componentId,
            ComponentId = componentDto.ComponentId,
            TopicId = componentDto.TopicId,
            DeviceId = componentDto.DeviceId,
            Label = componentDto.Label,
            Width = componentDto.Size.Width,
            Height = componentDto.Size.Height,
            PosX = (int) componentDto.Position.X,
            PosY = (int) componentDto.Position.Y
        };
    }

    public static UpdatePositionedComponentEntity ToEntity(this UpdatePositionedComponentDto componentDto)
    {
        return new UpdatePositionedComponentEntity
        {
            TopicId = componentDto.TopicId,
            Label = componentDto.Label,
            DeviceId = componentDto.DeviceId,
            Width = componentDto.Size?.Width,
            Height = componentDto.Size?.Height,
            PosX = componentDto.Position == null ? null : (int) componentDto.Position.X,
            PosY = componentDto.Position == null ? null : (int) componentDto.Position.Y
        };
    }

    public static PositionedComponentDto ToDto(this PositionedComponentEntity componentEntity)
    {
        Position position = new Position { X = componentEntity.PosX, Y = componentEntity.PosY };
        Size size = new Size { Width = componentEntity.Width, Height = componentEntity.Height };
        return new PositionedComponentDto
        {
            Id = componentEntity.Id,
            ComponentId = componentEntity.ComponentId,
            TopicId = componentEntity.TopicId,
            DeviceId = componentEntity.DeviceId,
            Label = componentEntity.Label,
            Size = size,
            Position = position
        };
    }
}