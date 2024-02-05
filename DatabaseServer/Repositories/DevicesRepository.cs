using Firebase;
using Domain.DeviceInfoDtos;

namespace DatabaseServer.Repositories;

internal sealed class DevicesRepository : IDevicesRepository
{
    private readonly FirebaseService firebaseService;

    public DevicesRepository(FirebaseService firebaseService)
    {
        this.firebaseService = firebaseService;
    }

    public async Task<DeviceDto?> GetDevice(string deviceId, CancellationToken cancellationToken)
    {
        return await firebaseService.GetDevice(deviceId, cancellationToken);
    }

    public async Task<DeviceDto> AddDevice(CreateDeviceDto device, CancellationToken cancellationToken)
    {
        return await firebaseService.AddDevice(device, cancellationToken);
    }

    public async Task UpdateDevice(string deviceId, UpdateDeviceDto device, CancellationToken cancellationToken)
    {
        await firebaseService.UpdateDevice(deviceId, device, cancellationToken);
    }

    public async Task DeleteDevice(string deviceId, CancellationToken cancellationToken)
    {
        await firebaseService.DeleteDevice(deviceId, cancellationToken);
    }

    public async Task<IDictionary<string, DeviceDto?>> GetDevices(IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {
        IDictionary<string, DeviceDto> devices = await firebaseService.GetDevicesForIds(ids, cancellationToken);
        // Since we want to return a value for all the ids, we insert null-values for all the ids that weren't found.
        return ids
             .ToDictionary(keySelector: id => id,
                 elementSelector: id => devices.TryGetValue(id, out var device) ? device : null);
    }
}