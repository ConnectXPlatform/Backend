namespace Firebase.Entities.Topic;

public class UpdateTopicEntity
{
    public string? Topic { get; init; }
    public string? DataType { get; init; }
    public int? ModeFlags { get; init; }
}