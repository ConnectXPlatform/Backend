using Firebase.CollectionHandlers;
using Firebase.Entities;
using Firebase.Entities.DeviceInfoEntities;
using Firebase.Mappers;
using Google.Cloud.Firestore;
using Grpc.Core;
using Domain.Components;
using Domain.ControlPanelDtos;
using Domain.DeviceInfoDtos;
using Domain.TopicDtos;
using Domain.UserDtos;
using FirebaseAdmin.Auth;
using FirebaseAdmin;

namespace Firebase;

public sealed class FirebaseService
{
    private FirestoreDb? firestoreDb;
    private FirebaseApp? firebaseApp;

    public async Task Initialize(string projectId)
    {
        firebaseApp = FirebaseApp.DefaultInstance ?? FirebaseApp.Create();
        firestoreDb = await FirestoreDb.CreateAsync(projectId);
    }

    #region Device

    public async Task<DeviceDto> AddDevice(CreateDeviceDto device, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.AddDevice(device, cancellationToken);
    }

    public async Task<DeviceDto?> GetDevice(string deviceId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.GetDevice(deviceId, cancellationToken);
    }

    public async Task UpdateDevice(string deviceId, UpdateDeviceDto update,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.UpdateDevice(deviceId, update, cancellationToken);
    }

    public async Task DeleteDevice(string deviceId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.DeleteDevice(deviceId, cancellationToken);
    }

    public async Task<IDictionary<string, DeviceDto>> GetDevicesForIds(IEnumerable<string> ids,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        IEnumerable<DeviceDto> devices = await firestoreDb.GetDevices(ids, cancellationToken);
        return devices.ToDictionary(pair=>pair.Id);
    }

    #endregion

    #region User

    public async Task<UserCreatedDto?> CreateUser(NewUserDto newUserDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        UserDto? user = null;
        DeviceDto? device = null;

        try
        {
            await firestoreDb.RunTransactionAsync(async transaction =>
            {
                DocumentReference deviceDoc = transaction.Database.NewDeviceDocument();
                DeviceInfoEntity deviceInfoEntity = new CreateDeviceDto
                {
                    Name = newUserDto.DeviceName,
                    Description = newUserDto.DeviceDescription
                }.ToEntity(deviceDoc.Id);
                var entityData = ObjectUtils.ToDictionary(deviceInfoEntity);
                transaction.Create(deviceDoc, entityData);
                device = deviceInfoEntity.ToDto();

                user = await transaction.Database.AddUser(new CreateUser
                {
                    Id = newUserDto.UserId,
                    Name = newUserDto.UserName,
                    Devices = new List<string> { deviceDoc.Id }
                }, transaction.CancellationToken);
            }, cancellationToken: cancellationToken);
        }
        catch (RpcException e) when (e.Status.StatusCode == StatusCode.Cancelled)
        {
            return null; // CancellationToken was cancelled
        }
        catch (RpcException e) when (e.Status.StatusCode == StatusCode.AlreadyExists)
        {
            return null;
        }

        if (user is null || device is null)
            return null;

        return new UserCreatedDto
        {
            User = user,
            Device = device
        };
    }

    public async Task<UserDto?> GetUser(string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.GetUser(userId, cancellationToken);
    }

    public async Task UpdateUser(string userId, UpdateUserDto userDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.UpdateUser(userId, userDto, cancellationToken);
    }

    public async Task DeleteUser(string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.DeleteUser(userId, cancellationToken);
    }

    #endregion

    #region Topic

    public async Task<TopicDto> AddTopic(CreateTopicDto topicDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.AddTopic(topicDto, cancellationToken);
    }

    public async Task<TopicDto?> GetTopic(string topicId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        TopicDto? topicDto = await firestoreDb.GetTopic(topicId, cancellationToken);
        return topicDto;
    }

    public async Task<TopicDto?> FindTopic(string topic, string parentDevice, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        DeviceDto? deviceDto = await firestoreDb.GetDevice(parentDevice, cancellationToken);
        if (deviceDto is null) return null;
        IList<string> ids = deviceDto.Topics;
        return await firestoreDb.FindTopic(topic, ids, cancellationToken);
    }

    public async Task<IDictionary<string, TopicDto>> GetTopicsForIds(IEnumerable<string> ids,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);

        IEnumerable<TopicDto> topics = await firestoreDb.GetTopics(ids, cancellationToken);
        return topics.ToDictionary(pair => pair.Id);
    }

    public async Task UpdateTopic(string topicId, UpdateTopicDto topicDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.UpdateTopic(topicId, topicDto, cancellationToken);
    }

    public async Task DeleteTopic(string topicId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.DeleteTopic(topicId, cancellationToken);
    }

    #endregion

    #region Component

    public async Task<ComponentDto> AddComponent(CreateComponentDto componentDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.AddComponent(componentDto, cancellationToken);
    }

    public async Task<ComponentDto?> GetComponent(string componentId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.GetComponent(componentId, cancellationToken);
    }

    public async Task<ICollection<ComponentDto>> GetAllComponents(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.GetAllComponents(cancellationToken);
    }

    public async Task DeleteComponent(string componentId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.DeleteComponent(componentId, cancellationToken);
    }

    #endregion

    #region Positioned component

    public async Task<PositionedComponentDto> AddPositionedComponent(CreatePositionedComponentDto componentDto,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.AddPositionedComponent(componentDto, cancellationToken);
    }

    public async Task<PositionedComponentDto?> GetPositionedComponent(string componentId,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.GetPositionedComponent(componentId, cancellationToken);
    }

    public async Task<IDictionary<string, PositionedComponentDto>> GetPositionedComponents(IEnumerable<string> ids,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        IEnumerable<PositionedComponentDto> components = await firestoreDb.GetPositionedComponents(ids, cancellationToken);
        return components.ToDictionary(pair => pair.Id);
    }

    public async Task UpdatePositionedComponent(string componentId, UpdatePositionedComponentDto update,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.UpdatePositionedComponent(componentId, update, cancellationToken);
    }

    public async Task DeletePositionedComponent(string componentId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.DeletePositionedComponent(componentId, cancellationToken);
    }

    #endregion

    #region Control panels

    public async Task<ControlPanelDto> AddControlPanel(CreateControlPanelDto controlPanelDto,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.AddControlPanel(controlPanelDto, cancellationToken);
    }

    public async Task<ControlPanelDto?> GetControlPanel(string controlPanelId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        return await firestoreDb.GetControlPanel(controlPanelId, cancellationToken);
    }

    public async Task<IDictionary<string, ControlPanelDto>> GetControlPanelsForIds(IEnumerable<string> ids,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        IEnumerable<ControlPanelDto> panels = await firestoreDb.GetControlPanels(ids, cancellationToken);
        return panels.ToDictionary(pair => pair.Id);
    }

    public async Task UpdateControlPanel(string controlPanelId, UpdateControlPanelDto update,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.UpdateControlPanel(controlPanelId, update, cancellationToken);
    }

    public async Task DeleteControlPanel(string controlPanelId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        await firestoreDb.DeleteControlPanel(controlPanelId, cancellationToken);
    }

    #endregion

    public async Task<bool> ValidateCredentials(string deviceId, string idToken)
    {
        ArgumentNullException.ThrowIfNull(firestoreDb);
        FirebaseToken token;
        try
        {
            token = await FirebaseAuth.GetAuth(firebaseApp).VerifyIdTokenAsync(idToken);
        }
        catch (FirebaseAuthException)
        {
            return false;
        }
        // After we made sure the token is valid, we check that the account is verified
        return bool.Parse(token.Claims.GetValueOrDefault("email_verified", bool.FalseString).ToString()!)
            //  And that the given device is registered in the database and associated with the token's user
            && await firestoreDb.UserHasDevice(token.Uid, deviceId);
    }
}