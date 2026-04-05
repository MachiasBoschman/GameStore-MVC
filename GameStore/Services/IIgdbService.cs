using GameStore.IgdbModels;

namespace GameStore.Services
{
    public interface IIgdbService
    {
        Task<IgdbGame?> SearchGameAsync(string name);
    }
}