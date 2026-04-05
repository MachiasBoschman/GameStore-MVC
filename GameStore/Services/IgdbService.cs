using GameStore.IgdbModels;
using GameStore.Options;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace GameStore.Services
{
    public class IgdbService : IIgdbService
    {
        private readonly HttpClient _httpClient;
        private readonly IgdbOptions _options;

        // Token cache
        private string? _accessToken;
        private DateTime _tokenExpiry = DateTime.MinValue;

        public IgdbService(HttpClient httpClient, IOptions<IgdbOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        private async Task EnsureTokenAsync()
        {
            Console.WriteLine($"TEST ClientId: {_options.ClientId}");
            Console.WriteLine($"TEST ClientSecret: {_options.ClientSecret}");

            // If token is still valid, do nothing
            if (_accessToken != null && DateTime.UtcNow < _tokenExpiry)
                return;

            var response = await _httpClient.PostAsync(
                $"https://id.twitch.tv/oauth2/token" +
                $"?client_id={_options.ClientId}" +
                $"&client_secret={_options.ClientSecret}" +
                $"&grant_type=client_credentials",
                null);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            _accessToken = doc.RootElement.GetProperty("access_token").GetString();
            int expiresIn = doc.RootElement.GetProperty("expires_in").GetInt32();
            // 60s buffer, a tiny amount of time can pass between the check and the call since code is async
            _tokenExpiry = DateTime.UtcNow.AddSeconds(expiresIn - 60); 
        }

        public async Task<IgdbGame?> SearchGameAsync(string name)
        {
            await EnsureTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.igdb.com/v4/games");
            request.Headers.Add("Client-ID", _options.ClientId);
            request.Headers.Add("Authorization", $"Bearer {_accessToken}");
            request.Content = new StringContent(
                $"fields name, summary, genres.name, platforms.name, cover.url; search \"{name}\"; limit 1;",
                Encoding.UTF8,
                "text/plain");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var results = JsonSerializer.Deserialize<List<IgdbGame>>(json);

            return results?.FirstOrDefault();
        }
    }
}