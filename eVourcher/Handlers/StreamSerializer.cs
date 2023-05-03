using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace eVoucher.Handlers
{
    public static class StreamSerializer
    {
        public static async Task<TValue> DeserializeAsync<TValue>(Stream utf8Json)
        {
            if (utf8Json == null || (utf8Json != null && utf8Json.Length < 1))
                return default(TValue);

            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() }
            };

            return await JsonSerializer.DeserializeAsync<TValue>(utf8Json, jsonSerializerOptions);
        }
    }
}
