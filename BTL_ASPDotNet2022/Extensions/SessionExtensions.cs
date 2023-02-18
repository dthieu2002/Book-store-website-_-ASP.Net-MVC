using System.Text.Json;

namespace BTL_ASPDotNet2022.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            string s = JsonSerializer.Serialize(value);
            session.SetString(key, s);
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
