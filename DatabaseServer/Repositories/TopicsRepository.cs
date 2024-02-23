using Firebase;
using Domain.TopicDtos;

namespace DatabaseServer.Repositories;

internal sealed class TopicsRepository : ITopicsRepository
{
    private readonly FirebaseService firebaseService;

    public TopicsRepository(FirebaseService firebaseService)
    {
        this.firebaseService = firebaseService;
    }

    public async Task<TopicDto?> GetTopic(string topicId, CancellationToken cancellationToken)
    {
        return await firebaseService.GetTopic(topicId, cancellationToken);
    }

    public async Task<TopicDto> CreateTopic(CreateTopicDto topicDto, CancellationToken cancellationToken)
    {
        return await firebaseService.AddTopic(topicDto, cancellationToken);
    }

    public async Task DeleteTopic(string topicId, CancellationToken cancellationToken)
    {
        await firebaseService.DeleteTopic(topicId, cancellationToken);
    }

    public async Task UpdateTopic(string topicId, UpdateTopicDto update, CancellationToken cancellationToken)
    {
        await firebaseService.UpdateTopic(topicId, update, cancellationToken);
    }

    public async Task<IDictionary<string, TopicDto?>> GetTopics(ISet<string> ids, CancellationToken cancellationToken)
    {
        var topics = await firebaseService.GetTopicsForIds(ids, cancellationToken);
        return ids
             .ToDictionary(keySelector: id => id,
                 elementSelector: id => topics.TryGetValue(id, out var topic) ? topic : null);
    }

    public async Task<TopicDto?> FindTopic(string topic, string parentDevice, CancellationToken cancellationToken)
    {
        return await firebaseService.FindTopic(topic, parentDevice, cancellationToken);
    }
}