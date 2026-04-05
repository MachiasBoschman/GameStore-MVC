using System.Text.Json.Serialization;

namespace GameStore.IgdbModels
{
    public class IgdbCover
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } = null!;
    }
}