using GameStore.Data;
using GameStore.Models;
using GameStore.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services
{
    // Separation of concerns, logic between Db and Controller is handled here
    public class GameService : IGameService
    {
        private readonly GameStoreContext _context;

        public GameService(GameStoreContext context)
        {
            _context = context;
        }
        // Create a game with data from the GameFormViewModel 
        public async Task CreateAsync(Game game, List<int> genreIds, List<int> platformIds, List<int> distributorIds)
        {
            await AttachRelations(game, genreIds, platformIds, distributorIds);
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }
        // Fetch a game by id
        public async Task<Game?> GetByIdAsync(int id) => 
            await _context.Games
                .Include(g => g.Genres)
                .Include(g => g.SteamApp)
                .Include(g => g.Platforms)
                .Include(g => g.Distributors).FirstOrDefaultAsync(g => g.Id == id);
        // Update a game, if it doesnt exist return false
        public async Task<bool> UpdateAsync(GameFormViewModel vm)
        {
            var existing = await _context.Games
                .Include(g => g.Genres)
                .Include(g => g.Platforms)
                .Include(g => g.Distributors)
                .FirstOrDefaultAsync(g => g.Id == vm.Game.Id);

            if (existing == null) return false;

            existing.Name = vm.Game.Name;
            existing.Price = vm.Game.Price;
            if (vm.ImageFile != null)
            {
                var fileName = Path.GetFileName(vm.ImageFile.FileName);
                var savePath = Path.Combine("wwwroot/images", fileName);
                using var stream = System.IO.File.Create(savePath);
                await vm.ImageFile.CopyToAsync(stream);
                existing.ImagePath = "/images/" + fileName;
            }
            await AttachRelations(existing, vm.GenreIds, vm.PlatformIds, vm.DistributorIds);
            await _context.SaveChangesAsync();
            return true;
        }
        // Delete a game, if it doesnt exist return false
        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game == null) return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }
        // Fetch all games
        public async Task<List<Game>> GetAllAsync() =>
            await _context.Games
                .Include(g => g.SteamApp)
                .Include(g => g.Genres)
                .Include(g => g.Platforms)
                .Include(g => g.Distributors).ToListAsync();

        // Helper method to assign dropdown choices to a game
        private async Task AttachRelations(Game game, List<int> genreIds, List<int> platformIds, List<int> distributorIds)
        {
            game.Genres = await _context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync();
            game.Platforms = await _context.Platforms.Where(p => platformIds.Contains(p.Id)).ToListAsync();
            game.Distributors = await _context.Distributors.Where(d => distributorIds.Contains(d.Id)).ToListAsync(); 

        }


        // Populate dropdowns for creating or editing views
        public async Task PopulateDropdownsAsync(GameFormViewModel viewModel)
        {
            viewModel.Genres = new SelectList(await _context.Genres.ToListAsync(), "Id", "Name");
            viewModel.Platforms = new SelectList(await _context.Platforms.ToListAsync(), "Id", "Name");
            viewModel.Distributors = new SelectList(await _context.Distributors.ToListAsync(), "Id", "Name");
        }
        // Populate the view model's previous selections by going through the items relation
        // Only applicable for edit views
        // shows the relations that had been selected during creation last time
        public void RetrieveRelationsAsync(Game game, GameFormViewModel vm)
        {
            vm.Game = game;
            vm.GenreIds = vm.Game.Genres.Select(g => g.Id).ToList();
            vm.PlatformIds = vm.Game.Platforms.Select(p => p.Id).ToList();
            vm.DistributorIds = vm.Game.Distributors.Select(d => d.Id).ToList();
        }
    }
}
