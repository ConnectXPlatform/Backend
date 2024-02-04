using Domain;
using Google.Cloud.Firestore;

namespace Firebase.Mappers;

internal static class CollectionUpdateMapper
{
    public static object? ToeUpdateObject(this CollectionUpdateDto? collection)
    {
        if (collection is null) return null;
        return collection.UpdateOperation switch
        {
            CollectionUpdateOperation.Add =>
                FieldValue.ArrayUnion(collection.Update.Cast<object>().ToArray()),
            CollectionUpdateOperation.Delete =>
                FieldValue.ArrayRemove(collection.Update.Cast<object>().ToArray()),
            CollectionUpdateOperation.Set =>
                collection.Update,
            _ => null
        };
    }
}