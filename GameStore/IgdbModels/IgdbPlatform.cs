using System.Text.Json.Serialization;

namespace GameStore.IgdbModels
{
    public class IgdbPlatform
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }
}