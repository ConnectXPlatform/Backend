using Domain.TopicDtos;

namespace DatabaseServer.Repositories;

internal interface ITopicsRepository
{
    Task<TopicDto?> GetTopic(string topicId, CancellationToken cancellationToken);
    Task<TopicDto> CreateTopic(CreateTopicDto topicDto, CancellationToken cancellationToken);
    Task DeleteTopic(string topicId, CancellationToken cancellationToken);
    Task UpdateTopic(string topicId, UpdateTopicDto update, CancellationToken cancellationToken);
    Task<IDictionary<string, TopicDto?>> GetTopics(ISet<string> ids, CancellationToken cancellationToken);
    Task<TopicDto?> FindTopic(string topic, string parentDevice, CancellationToken cancellationToken);
}