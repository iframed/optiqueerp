using System.Text.Json;
using System.Text.Json.Serialization;

public static class SessionExtensions
{
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        // Utilisez JsonSerializer au lieu de JsonConverter pour la sérialisation
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        // Utilisez JsonSerializer au lieu de JsonConvert pour la désérialisation
        return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
    }
}
//12345abcd@Aefs