using System.Text.Json.Serialization;

namespace GameStore.IgdbModels
{
    public class IgdbGame
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("cover")]
        public IgdbCover? Cover { get; set; }

        [JsonPropertyName("genres")]
        public List<IgdbGenre> Genres { get; set; } = new();

        [JsonPropertyName("platforms")]
        public List<IgdbPlatform> Platforms { get; set; } = new();
    }
}