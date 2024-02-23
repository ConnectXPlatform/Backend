using DatabaseServer.Repositories;
using Domain;
using Domain.ControlPanelDtos;
using Domain.DeviceInfoDtos;
using Domain.UserDtos;

namespace DatabaseServer.Endpoints;

internal static class UsersComponents
{
    public static T MapUsersComponentsEndpoints<T>(this T endpointRouteBuilder) where T : IEndpointRouteBuilder
    {
        endpointRouteBuilder.MapPost("/{userId}/devices", AddDevice)
            .Produces<DeviceDto>();
        endpointRouteBuilder.MapDelete("/{userId}/devices/{deviceId}", DeleteDevice);
        endpointRouteBuilder.MapPost("/{userId}/control_panels", AddControlPanel)
            .Produces<ControlPanelDto>(StatusCodes.Status201Created);
        endpointRouteBuilder.MapDelete("/{userId}/control_panels/{controlPanelId}", DeleteControlPanel);
        return endpointRouteBuilder;
    }

    private static async Task<IResult> AddDevice(IUsersRepository usersRepository, IDevicesRepository devicesRepository,
        string userId, CreateDeviceDto deviceDto, CancellationToken cancellationToken)
    {
        DeviceDto created = await devicesRepository.AddDevice(deviceDto, cancellationToken);
        await usersRepository.UpdateUser(userId, new UpdateUserDto
        {
            Devices = new CollectionUpdateDto
            {
                Update = new HashSet<string> { created.Id },
                UpdateOperation = CollectionUpdateOperation.Add
            }
        }, cancellationToken);
        return Results.CreatedAtRoute("GetDevice", new { deviceId = created.Id }, created);
    }

    private static async Task<IResult> DeleteDevice(IUsersRepository usersRepository,
        IDevicesRepository devicesRepository, string userId, string deviceId, CancellationToken cancellationToken)
    {
        await devicesRepository.DeleteDevice(deviceId, cancellationToken);
        await usersRepository.UpdateUser(userId, new UpdateUserDto
        {
            Devices = new CollectionUpdateDto
            {
                Update = new HashSet<string> { deviceId },
                UpdateOperation = CollectionUpdateOperation.Delete
            }
        }, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> AddControlPanel(IUsersRepository usersRepository,
        IControlPanelsRepository controlPanelsRepository, string userId, CreateControlPanelDto controlPanel,
        CancellationToken cancellationToken)
    {
        ControlPanelDto created = await controlPanelsRepository.AddControlPanel(controlPanel, cancellationToken);

        await usersRepository.UpdateUser(userId, new UpdateUserDto
        {
            ControlPanels = new CollectionUpdateDto
            {
                Update = new HashSet<string> { created.Id },
                UpdateOperation = CollectionUpdateOperation.Add
            }
        }, cancellationToken);
        return Results.CreatedAtRoute("GetControlPanel", new { controlPanelId = created.Id }, created);
    }

    private static async Task<IResult> DeleteControlPanel(IUsersRepository usersRepository,
        IControlPanelsRepository controlPanelsRepository, string userId, string controlPanelId,
        CancellationToken cancellationToken)
    {
        await controlPanelsRepository.DeleteControlPanel(controlPanelId, cancellationToken);
        await usersRepository.UpdateUser(userId, new UpdateUserDto
        {
            ControlPanels = new CollectionUpdateDto
            {
                Update = new HashSet<string> { controlPanelId },
                UpdateOperation = CollectionUpdateOperation.Delete
            }
        }, cancellationToken);
        return Results.NoContent();
    }
}