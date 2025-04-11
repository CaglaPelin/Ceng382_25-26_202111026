using System.Text.Json;
using System.Text;

namespace MyRazorApp.Helpers
{
    public sealed class JsonUtils
    {
        private static readonly Lazy<JsonUtils> lazy = new(() => new JsonUtils());

        public static JsonUtils Instance => lazy.Value;

        private JsonUtils() { }

        public string Serialize<T>(List<T> data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(data, options);
        }

        public byte[] SerializeToBytes<T>(List<T> data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(data, options);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
