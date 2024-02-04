namespace Domain;
public enum CollectionUpdateOperation
{
    Set,
    Add,
    Delete
}
public sealed class CollectionUpdateDto
{
    public required ISet<string> Update { get; init; }
    public required CollectionUpdateOperation UpdateOperation { get; init; }
}