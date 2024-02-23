using System.Text;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using MqttBroker;
using Firebase;
using Microsoft.Extensions.Logging;

var mqttFactory = new MqttFactory();
var mqttServerOptions = new MqttServerOptionsBuilder()
    .WithDefaultEndpoint()
    .Build();

var firebaseService = new FirebaseService();
await firebaseService.Initialize(Environment.GetEnvironmentVariable("PROJECT_NAME")
    ?? throw new ApplicationException("Project name not defined in environment variable 'PROJECT_NAME'"));

using var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions);
// Source: https://stackoverflow.com/questions/60688112/logging-in-console-application-net-core-with-di
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter(typeof(Program).FullName, LogLevel.Trace)
        .AddConsole();
});
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

mqttServer.InterceptingPublishAsync += args =>
{
    logger.LogInformation("{Client} published on \"{Topic}\" (Retain = {Retain})", args.ClientId, args.ApplicationMessage.Topic, args.ApplicationMessage.Retain);

    return Task.CompletedTask;
};
mqttServer.ClientSubscribedTopicAsync += args =>
{
    logger.LogInformation("Client {name} subscribed to {topic}", args.ClientId, args.TopicFilter.Topic);
    return Task.CompletedTask;
};
mqttServer.ClientUnsubscribedTopicAsync += args =>
{
    logger.LogInformation("Client {name} unsubscribed from {topic}", args.ClientId, args.TopicFilter);
    return Task.CompletedTask;
};
mqttServer.ClientConnectedAsync += args =>
{
    (string userId, string deviceId) = IdUtils.ExtractUserAndDeviceIdsFrom(args.ClientId);
    logger.LogInformation("User {name} connected with device {device}", userId, deviceId);
    return Task.CompletedTask;
};
mqttServer.ClientDisconnectedAsync += async args =>
{
    var (userId, deviceId) = IdUtils.ExtractUserAndDeviceIdsFrom(args.ClientId);
    logger.LogInformation("User {name} disconnected with device {device}", userId, deviceId);

    await mqttServer.InjectApplicationMessage($"{userId}/ALL/{deviceId}/json/Connected", retain: true);
    await mqttServer.InjectApplicationMessage($"{userId}/ALL/{deviceId}/json/Disconnected", retain: false);
};

mqttServer.ValidatingConnectionAsync += async args =>
{
    // The username passed to the server is the device's id in the database, and the password is an id token
    logger.LogInformation("Connection attempt {username} from {endpoint}", args.UserName, args.Endpoint);
    args.ReasonCode = MqttConnectReasonCode.Success;
    if (!await firebaseService.ValidateCredentials(args.UserName, args.Password))
    {
        args.ReasonCode = MqttConnectReasonCode.NotAuthorized;
        args.ReasonString = "Unauthorized";
        logger.LogDebug("Unauthorized!");
    }
};
await mqttServer.StartAsync();

logger.LogInformation("Listening on {address}:{port}", mqttServerOptions.DefaultEndpointOptions.BoundInterNetworkAddress,
    mqttServerOptions.DefaultEndpointOptions.Port);
while (true) { Thread.Sleep(1); }