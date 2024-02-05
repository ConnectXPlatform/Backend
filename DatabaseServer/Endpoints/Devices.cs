using DatabaseServer.Repositories;
using Domain.DeviceInfoDtos;

namespace DatabaseServer.Endpoints;

internal static class Devices
{
    public static T MapDevicesEndpoints<T>(this T endpointRouteBuilder) where T : IEndpointRouteBuilder
    {
        endpointRouteBuilder.MapGet("/{deviceId}", GetDevice)
            .Produces<DeviceDto>()
            .WithName(nameof(GetDevice));
        // Should be a GET request, but usually GET requests don't contain a body.
        endpointRouteBuilder.MapPost("/map", GetDevices)
            .Produces<IDictionary<string, DeviceDto>>();
        endpointRouteBuilder.MapPost("/", AddDevice)
            .Produces<DeviceDto>(StatusCodes.Status201Created);
        endpointRouteBuilder.MapPut("/{deviceId}", UpdateDevice);
        endpointRouteBuilder.MapDelete("/{deviceId}", DeleteDevice);
        return endpointRouteBuilder;
    }

    private static async Task<IResult> GetDevice(IDevicesRepository devicesRepository, string deviceId,
        CancellationToken cancellationToken)
    {
        DeviceDto? device = await devicesRepository.GetDevice(deviceId, cancellationToken);
        return device is null
            ? Results.NotFound()
            : Results.Ok(device);
    }

    private static async Task<IResult> GetDevices(IDevicesRepository devicesRepository, ISet<string> devicesIds,
        CancellationToken cancellationToken)
    {
        return Results.Ok(await devicesRepository.GetDevices(devicesIds, cancellationToken));
    }

    private static async Task<IResult> AddDevice(IDevicesRepository devicesRepository, CreateDeviceDto device,
        HttpContext httpContext, CancellationToken cancellationToken)
    {
        DeviceDto createdDevice = await devicesRepository.AddDevice(device, cancellationToken);
        return Results.CreatedAtRoute(nameof(GetDevice), new { deviceId = createdDevice.Id }, createdDevice);
    }

    private static async Task<IResult> UpdateDevice(IDevicesRepository devicesRepository, string deviceId,
        UpdateDeviceDto update, CancellationToken cancellationToken)
    {
        await devicesRepository.UpdateDevice(deviceId, update, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteDevice(IDevicesRepository devicesRepository, string deviceId,
        CancellationToken cancellationToken)
    {
        await devicesRepository.DeleteDevice(deviceId, cancellationToken);
        return Results.NoContent();
    }
}