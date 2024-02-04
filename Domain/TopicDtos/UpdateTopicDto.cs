namespace Domain.TopicDtos;

public sealed class UpdateTopicDto
{
    public string? Topic { get; init; }
    public string? DataType { get; init; }
    public TopicModes? ModeFlags { get; init; }
}