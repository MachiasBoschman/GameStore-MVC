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
        public async Task CreateAsync(GameFormViewModel vm)
        {
            await AttachRelations(vm);
            _context.Games.Add(vm.Game);
            await _context.SaveChangesAsync();
        }
        // Fetch a game by id
        public async Task<Game?> GetByIdAsync(int id) { 
            var game = await _context.Games
                .Include(g => g.Genres)
                .Include(g => g.SteamApp)
                .Include(g => g.Platforms)
                .Include(g => g.Distributors).FirstOrDefaultAsync(g => g.Id == id);

            if (game != null && game.ImagePath != null && !File.Exists("wwwroot" + game.ImagePath))
            {
                game.ImagePath = null;
            }

            return game;
        }
        // Update a game, if it doesnt exist return false
        public async Task<bool> UpdateAsync(GameFormViewModel vm)
        {
            var existing = await _context.Games
                .Include(g => g.Genres)
                .Include(g => g.Platforms)
                .Include(g => g.Distributors)
                .FirstOrDefaultAsync(g => g.Id == vm.Game.Id);

            if (existing == null) return false;

            await AttachRelations(existing, vm);
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
        public async Task<List<Game>> GetAllAsync() { 
        
            var games = await _context.Games
                .Include(g => g.SteamApp)
                .Include(g => g.Genres)
                .Include(g => g.Platforms)
                .Include(g => g.Distributors).ToListAsync();

            foreach (var game in games)
            {
                if (game.ImagePath != null && !File.Exists("wwwroot" + game.ImagePath))
                {
                    game.ImagePath = null;
                }
            }
            return games;
        }

        // Helper method to assign dropdown choices to a game
        private async Task AttachRelations(GameFormViewModel vm)
        {
            vm.Game.Genres = await _context.Genres.Where(g => vm.GenreIds.Contains(g.Id)).ToListAsync();
            vm.Game.Platforms = await _context.Platforms.Where(p => vm.PlatformIds.Contains(p.Id)).ToListAsync();
            vm.Game.Distributors = await _context.Distributors.Where(d => vm.DistributorIds.Contains(d.Id)).ToListAsync();
            if (vm.ImageFile != null)
            {
                var fileName = Path.GetFileName(vm.ImageFile.FileName);
                var savePath = Path.Combine("wwwroot/images", fileName);
                using var stream = System.IO.File.Create(savePath);
                await vm.ImageFile.CopyToAsync(stream);
                vm.Game.ImagePath = "/images/" + fileName;
            }
        }

        // For Update — targets the tracked EF entity instead
        private async Task AttachRelations(Game existing, GameFormViewModel vm)
        {
            existing.Name = vm.Game.Name;
            existing.Price = vm.Game.Price;
            existing.Genres = await _context.Genres.Where(g => vm.GenreIds.Contains(g.Id)).ToListAsync();
            existing.Platforms = await _context.Platforms.Where(p => vm.PlatformIds.Contains(p.Id)).ToListAsync();
            existing.Distributors = await _context.Distributors.Where(d => vm.DistributorIds.Contains(d.Id)).ToListAsync();

            if (vm.ImageFile != null)
            {
                var fileName = Path.GetFileName(vm.ImageFile.FileName);
                var savePath = Path.Combine("wwwroot/images", fileName);
                using var stream = System.IO.File.Create(savePath);
                await vm.ImageFile.CopyToAsync(stream);
                existing.ImagePath = "/images/" + fileName;
            }
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
