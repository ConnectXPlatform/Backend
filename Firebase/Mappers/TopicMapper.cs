using Domain.TopicDtos;
using Firebase.Entities.Topic;

namespace Firebase.Mappers;

internal static class TopicMapper
{
    public static TopicEntity ToEntity(this TopicDto topicDto)
    {
        return new TopicEntity
        {
            Id = topicDto.Id,
            Topic = topicDto.Topic,
            DataType = topicDto.DataType,
            ModeFlags = (int)topicDto.ModeFlags
        };
    }

    public static TopicEntity ToEntity(this CreateTopicDto topicDto, string id)
    {
        return new TopicEntity
        {
            Id = id,
            Topic = topicDto.Topic,
            DataType = topicDto.DataType,
            ModeFlags = (int)topicDto.ModeFlags
        };
    }

    public static UpdateTopicEntity ToEntity(this UpdateTopicDto topicDto)
    {
        return new UpdateTopicEntity
        {
            Topic = topicDto.Topic,
            DataType = topicDto.DataType,
            ModeFlags = topicDto.ModeFlags == null ? null : (int)topicDto.ModeFlags
        };
    }

    public static TopicDto ToDto(this TopicEntity topicEntity)
    {
        return new TopicDto
        {
            Id = topicEntity.Id,
            Topic = topicEntity.Topic,
            DataType = topicEntity.DataType,
            ModeFlags = (TopicModes)topicEntity.ModeFlags
        };
    }
}