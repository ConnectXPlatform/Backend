using System.Collections.ObjectModel;

namespace Firebase.Entities;

internal class CreateUser
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public IList<string> Devices { get; init; } = ReadOnlyCollection<string>.Empty;
}