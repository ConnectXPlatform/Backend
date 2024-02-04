namespace Domain.TopicDtos;

public sealed class CreateTopicDto
{
    public required string Topic { get; init; }
    public required string DataType { get; init; }
    public required TopicModes ModeFlags { get; init; }
}