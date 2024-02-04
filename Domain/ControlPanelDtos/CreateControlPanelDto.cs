namespace Domain.ControlPanelDtos;

public sealed class CreateControlPanelDto
{
    public required string Name { get; init; }
    public required string Creator { get; init; }
    public string Description { get; init; } = string.Empty;
}