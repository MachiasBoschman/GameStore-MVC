using GameStore.Models;
using GameStore.ViewModels;

namespace GameStore.Services
{
    public interface IGameService
    {
        Task<List<Game>> GetAllAsync();
        Task<Game?> GetByIdAsync(int id);
        Task CreateAsync(GameFormViewModel vm);
        Task<bool> UpdateAsync(GameFormViewModel vm);
        Task<bool> DeleteAsync(int id);
        Task PopulateDropdownsAsync(GameFormViewModel viewModel);
        void RetrieveRelationsAsync(Game game, GameFormViewModel vm);
    }
}
