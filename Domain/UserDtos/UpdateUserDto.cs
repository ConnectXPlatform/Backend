namespace Domain.UserDtos;

public sealed class UpdateUserDto
{
    public string? Name { get; init; }
    public CollectionUpdateDto? Devices { get; init; }
    public CollectionUpdateDto? ControlPanels { get; init; }
}