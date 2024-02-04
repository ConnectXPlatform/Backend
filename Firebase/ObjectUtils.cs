using Utf8Json;
using Utf8Json.Resolvers;

namespace Firebase;

internal static class ObjectUtils
{
    private static readonly IJsonFormatterResolver Resolver;

    static ObjectUtils()
    {
        Resolver =
            CompositeResolver.Create(StandardResolver.AllowPrivateExcludeNullCamelCase, EnumResolver.UnderlyingValue);
    }

    public static IDictionary<string, object> ToDictionary(object obj)
    {
        string temp = JsonSerializer.ToJsonString(obj, Resolver);
        return JsonSerializer.Deserialize<Dictionary<string, object>>(temp, Resolver);
    }

    //public static T ToObject<T>(IDictionary<string, object> dictionary)
    //{
    //    string temp = JsonSerializer.ToJsonString(dictionary, Resolver);
    //    return JsonSerializer.Deserialize<T>(temp, Resolver);
    //}
}