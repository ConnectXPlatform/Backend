using Domain.TopicDtos;
using Firebase.Entities.Topic;
using Firebase.Mappers;
using Google.Cloud.Firestore;

namespace Firebase.CollectionHandlers;

internal static class TopicsCollection
{
    private const string CollectionId = "Topics";

    public static async Task<TopicDto?> GetTopic(this FirestoreDb db, string topicId,
        CancellationToken cancellationToken)
    {
        var snapshot = await db.Collection(CollectionId)
            .Document(topicId)
            .GetSnapshotAsync(cancellationToken);
        if (!snapshot.Exists)
            return null;

        var topicEntity = snapshot.ConvertTo<TopicEntity>();
        return topicEntity.ToDto();
    }

    public static async Task<IEnumerable<TopicDto>> GetTopics(this FirestoreDb db, IEnumerable<string> ids,
        CancellationToken cancellationToken)
    {
        QuerySnapshot? snapshot = await db.Collection(CollectionId)
            .WhereIn(FieldPath.DocumentId, ids)
            .GetSnapshotAsync(cancellationToken);

        return snapshot.Documents
            .Select(document =>
            {
                var topicEntity = document.ConvertTo<TopicEntity>();
                return topicEntity.ToDto();
            });
    }

    public static async Task<TopicDto?> FindTopic(this FirestoreDb db, string topic,
        IEnumerable<string> ids, CancellationToken cancellationToken)
    {
        var snapshot = await db.Collection(CollectionId)
            .WhereIn(FieldPath.DocumentId, ids)
            .WhereEqualTo("topic", topic)
            .Limit(1)
            .GetSnapshotAsync(cancellationToken);

        if (snapshot.Count == 0)
            return null;

        var topicEntity = snapshot.Documents[0].ConvertTo<TopicEntity>();
        return topicEntity.ToDto();
    }

    public static async Task<TopicDto> AddTopic(this FirestoreDb db, CreateTopicDto topic,
        CancellationToken cancellationToken)
    {
        CollectionReference collection = db.Collection(CollectionId);
        DocumentReference document = collection.Document();
        TopicEntity topicEntity = topic.ToEntity(document.Id);
        IDictionary<string, object> entityData = ObjectUtils.ToDictionary(topicEntity);
        await document.CreateAsync(entityData, cancellationToken);
        return topicEntity.ToDto();
    }

    public static async Task UpdateTopic(this FirestoreDb db, string topicId, UpdateTopicDto update,
        CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(topicId);
        var updates = ObjectUtils.ToDictionary(update.ToEntity());

        await document.UpdateAsync(updates, cancellationToken: cancellationToken);
    }

    public static async Task DeleteTopic(this FirestoreDb db, string topicId, CancellationToken cancellationToken)
    {
        DocumentReference document = db.Collection(CollectionId).Document(topicId);
        await document.DeleteAsync(cancellationToken: cancellationToken);
    }
}