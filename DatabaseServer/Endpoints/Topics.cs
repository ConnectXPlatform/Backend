using DatabaseServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Domain.TopicDtos;

namespace DatabaseServer.Endpoints;

public static class Topics
{
    public static T MapTopicsEndpoints<T>(this T endpointRouteBuilder) where T : IEndpointRouteBuilder
    {
        endpointRouteBuilder.MapGet("/{topicId}", GetTopic)
            .Produces<TopicDto>()
            .WithName(nameof(GetTopic));
        endpointRouteBuilder.MapPost("/map", GetTopics)
            .Produces<IDictionary<string, TopicDto?>>();
        endpointRouteBuilder.MapGet("/", FindTopic)
            .Produces<TopicDto?>();
        endpointRouteBuilder.MapPost("/", AddTopic)
            .Produces<TopicDto>(StatusCodes.Status201Created);
        endpointRouteBuilder.MapPut("/{topicId}", UpdateTopic);
        endpointRouteBuilder.MapDelete("/{topicId}", DeleteTopic);
        return endpointRouteBuilder;
    }

    private static async Task<IResult> GetTopic(ITopicsRepository topicsRepository, string topicId,
        CancellationToken cancellationToken)
    {
        TopicDto? topicDto = await topicsRepository.GetTopic(topicId, cancellationToken);
        if (topicDto is null) return Results.NotFound();
        return Results.Ok(topicDto);
    }

    private static async Task<IResult> GetTopics(ITopicsRepository topicsRepository, ISet<string> ids,
        CancellationToken cancellationToken)
    {
        return Results.Ok(await topicsRepository.GetTopics(ids, cancellationToken));
    }

    private static async Task<IResult> FindTopic(ITopicsRepository topicsRepository, [FromQuery] string topic,
        [FromQuery] string parentDevice, CancellationToken cancellationToken)
    {
        TopicDto? topicDto = await topicsRepository.FindTopic(topic, parentDevice, cancellationToken);
        if (topicDto is null) return Results.NotFound();
        return Results.Ok(topicDto);
    }

    private static async Task<IResult> AddTopic(ITopicsRepository topicsRepository, CreateTopicDto topicDto,
        CancellationToken cancellationToken)
    {
        var topic = await topicsRepository.CreateTopic(topicDto, cancellationToken);
        return Results.CreatedAtRoute(nameof(GetTopic), new { topicId = topic.Id }, topic);
    }

    private static async Task<IResult> UpdateTopic(ITopicsRepository topicsRepository, string topicId,
        UpdateTopicDto update, CancellationToken cancellationToken)
    {
        await topicsRepository.UpdateTopic(topicId, update, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteTopic(ITopicsRepository topicsRepository, string topicId,
        CancellationToken cancellationToken)
    {
        await topicsRepository.DeleteTopic(topicId, cancellationToken);
        return Results.NoContent();
    }
}