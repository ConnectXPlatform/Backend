namespace MqttBroker;

internal static class IdUtils
{
    private const char Separator = '_';

    public static (string userId, string deviceId) ExtractUserAndDeviceIdsFrom(string clientId)
    {
        // Client id format: <user_id>_<device_id>
        string[] split = clientId.Split(Separator);
        return (split[0], split[1]);
    }
}