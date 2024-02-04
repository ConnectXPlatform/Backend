using System.Collections.ObjectModel;

namespace Domain.ControlPanelDtos;

public sealed class ControlPanelDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Creator { get; init; }
    public string Description { get; init; } = string.Empty;
    public IList<string> Components { get; init; } = ReadOnlyCollection<string>.Empty;
}