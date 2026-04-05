using GameStore.IgdbModels;
using GameStore.ViewModels;

namespace GameStore.Services
{
    public interface IIgdbService
    {
        Task<IgdbGame?> SearchGameAsync(string name);
        Task<GameFormViewModel> MapToViewModelAsync(IgdbGame igdbGame);
    }
}