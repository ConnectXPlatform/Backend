namespace Domain.TopicDtos;

[Flags]
public enum TopicModes : byte
{
    DataProvider = 1 << 0,
    CommandsProcessor = 1 << 1,
    Both = DataProvider | CommandsProcessor
}

public sealed class TopicDto
{
    public required string Id { get; init; }
    public required string Topic { get; init; }
    public required string DataType { get; init; }
    public required TopicModes ModeFlags { get; init; }

    public bool WaitForResponse()
    {
        return ModeFlags.HasFlag(TopicModes.DataProvider);
    }
}