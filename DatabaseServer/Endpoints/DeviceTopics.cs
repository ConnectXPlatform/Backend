using DatabaseServer.Repositories;
using Domain;
using Domain.DeviceInfoDtos;
using Domain.TopicDtos;

namespace DatabaseServer.Endpoints;

internal static class DeviceTopics
{
    public static T MapDeviceTopicsEndpoints<T>(this T endpointRouteBuilder) where T : IEndpointRouteBuilder
    {
        endpointRouteBuilder.MapPost("/{deviceId}/topics", AddTopic)
            .Produces<TopicDto>(StatusCodes.Status201Created);
        endpointRouteBuilder.MapDelete("/{deviceId}/topics/{topicId}", DeleteTopic);
        return endpointRouteBuilder;
    }

    private static async Task<IResult> AddTopic(ITopicsRepository topicsRepository,
        IDevicesRepository devicesRepository, string deviceId, CreateTopicDto topicDto,
        CancellationToken cancellationToken)
    {
        TopicDto created = await topicsRepository.CreateTopic(topicDto, cancellationToken);
        await devicesRepository.UpdateDevice(deviceId, new UpdateDeviceDto
        {
            Topics = new CollectionUpdateDto
            {
                Update = new HashSet<string> { created.Id },
                UpdateOperation = CollectionUpdateOperation.Add
            }
        }, cancellationToken);
        return Results.Ok(created);
    }

    private static async Task<IResult> DeleteTopic(ITopicsRepository topicsRepository,
        IDevicesRepository devicesRepository, string deviceId, string topicId,
        CancellationToken cancellationToken)
    {
        await topicsRepository.DeleteTopic(topicId, cancellationToken);
        await devicesRepository.UpdateDevice(deviceId, new UpdateDeviceDto
        {
            Topics = new CollectionUpdateDto
            {
                Update = new HashSet<string> { topicId },
                UpdateOperation = CollectionUpdateOperation.Delete
            }
        }, cancellationToken);
        return Results.NoContent();
    }
}