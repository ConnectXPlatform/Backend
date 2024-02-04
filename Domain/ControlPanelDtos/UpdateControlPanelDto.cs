namespace Domain.ControlPanelDtos;

public sealed class UpdateControlPanelDto
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public CollectionUpdateDto? Components { get; init; }
}