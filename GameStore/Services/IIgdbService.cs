namespace GameStore.Services
{
    public interface IIgdbService
    {
        Task<string> SearchGameAsync(string name);
    }
}