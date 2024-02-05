using Domain.DeviceInfoDtos;

namespace DatabaseServer.Repositories;

internal interface IDevicesRepository
{
    Task<DeviceDto?> GetDevice(string deviceId, CancellationToken cancellationToken);
    Task<DeviceDto> AddDevice(CreateDeviceDto device, CancellationToken cancellationToken);
    Task UpdateDevice(string deviceId, UpdateDeviceDto device, CancellationToken cancellationToken);
    Task DeleteDevice(string deviceId, CancellationToken cancellationToken);
    Task<IDictionary<string, DeviceDto?>> GetDevices(IEnumerable<string> ids, CancellationToken cancellationToken);
}