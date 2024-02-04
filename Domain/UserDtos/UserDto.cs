using System.Collections.ObjectModel;

namespace Domain.UserDtos;

public sealed class UserDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public IList<string> Devices { get; init; } = ReadOnlyCollection<string>.Empty;
    public IList<string> ControlPanels { get; init; } = ReadOnlyCollection<string>.Empty;
}