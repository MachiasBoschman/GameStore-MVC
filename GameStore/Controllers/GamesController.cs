using GameStore.Data;
using GameStore.Models;
using GameStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Controllers
{
    public class GamesController : Controller
    {
        // Stores the context of the DB in the GamesController.
        private readonly GameStoreContext _context;

        // GamesController automatically requests a connection with the DB on creation.
        public GamesController(GameStoreContext context)
        {
            _context = context;
        }
        // Simple getter Index page, includes all connections from Games
        // And wraps it as a model for /Games/Index
        public async Task<IActionResult> Index()
        {
            var games = await _context.Games
                .Include(g => g.SteamApp)
                .Include(g => g.Genres)
                .Include(g => g.Platforms)
                .Include(g => g.Distributors).ToListAsync();
            return View(games);
        }
        // This action populates the dropdowns needed for
        // the empty form of the Create view.
        public async Task<IActionResult> Create()
        {
            var ViewModel = new GameFormViewModel();
            await PopulateDropdowns(ViewModel);
            return View(ViewModel);
        }
        // This handles the post functionality of the form
        [HttpPost]
        public async Task<IActionResult> Create(GameFormViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                // Adds all Genre connections in the temporary viewmodel class
                // Where GenreIds from the actual DB match with the chosen GenreIds from the forms
                ViewModel.Game.Genres = await _context.Genres
                    .Where(g => ViewModel.GenreIds.Contains(g.Id))
                    .ToListAsync();
                // Adds all Platform connections in the temporary viewmodel class
                // Where PlatformIds from the actual DB match with the chosen PlatformIds from the forms
                ViewModel.Game.Platforms = await _context.Platforms
                    .Where(p => ViewModel.PlatformIds.Contains(p.Id))
                    .ToListAsync();
                // Adds all Distributor connections in the temporary viewmodel class
                // Where DistributorIds from the actual DB match with the chosen DistributorIds from the forms
                ViewModel.Game.Distributors = await _context.Distributors
                    .Where(d => ViewModel.DistributorIds.Contains(d.Id))
                    .ToListAsync();
                // Finally add the temporary Game object into the actual DB
                _context.Games.Add(ViewModel.Game);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            // If validation failed, repopulate the dropdowns
            // Return the form once again, but with error messages
            // User has a chance to fix mistakes (previous input preserved)
            await PopulateDropdowns(ViewModel);
            return View(ViewModel);
        }
        // Return the model of the requested DB entry to the view
        public async Task<IActionResult> Edit(int id)
        {
            // Try to find a game in the DB with given ID from url
            var game = await _context.Games
                .Include(g => g.SteamApp)
                .Include(g => g.Genres)
                .Include(g => g.Platforms)
                .Include(g => g.Distributors).FirstOrDefaultAsync(g => g.Id == id);

            // If game is null, display error 404 response
            if (game == null) return NotFound();

            // Create FormViewModel for fluid interaction with forms
            var ViewModel = new GameFormViewModel();
            ViewModel.Game = game;
            ViewModel.GenreIds = ViewModel.Game.Genres.Select(g => g.Id).ToList(); 
            ViewModel.PlatformIds = ViewModel.Game.Platforms.Select(p => p.Id).ToList();
            ViewModel.DistributorIds = ViewModel.Game.Distributors.Select(d => d.Id).ToList();
            // Separate Lists for display in dropdowns
            await PopulateDropdowns(ViewModel);
            return View(ViewModel);
        }
        // Update the existing DB entry with the input fields from the view
        [HttpPost]
        public async Task<IActionResult> Edit(GameFormViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                var game = await _context.Games
                    .Include(g => g.Genres)
                    .Include(g => g.Platforms)
                    .Include(g => g.Distributors).FirstOrDefaultAsync(g => g.Id == ViewModel.Game.Id);

                if (game == null) return NotFound();

                game.Name = ViewModel.Game.Name;
                game.Price = ViewModel.Game.Price;

                game.Genres = await _context.Genres
                    .Where(g => ViewModel.GenreIds.Contains(g.Id))
                    .ToListAsync();
                game.Platforms = await _context.Platforms
                    .Where(p => ViewModel.PlatformIds.Contains(p.Id))
                    .ToListAsync();
                game.Distributors = await _context.Distributors
                    .Where(d => ViewModel.DistributorIds.Contains(d.Id))
                    .ToListAsync();

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await PopulateDropdowns(ViewModel);
            return View(ViewModel);
        }
        // Return the model of the requested DB entry to the view
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _context.Games
                .Include(g => g.Genres)
                .Include(g => g.SteamApp)
                .Include(g => g.Platforms)
                .Include(g => g.Distributors).FirstOrDefaultAsync(g => g.Id == id);
            return View(game);
        }
        // Confirm the deletion process, after pressing delete in the view.
        // Only delete if the DB entry still exists.
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == id);
            if(game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        // Helper method to populate dropdowns for creating or editing operations
        private async Task PopulateDropdowns(GameFormViewModel ViewModel)
        {
            ViewModel.Genres = new SelectList(await _context.Genres.ToListAsync(), "Id", "Name");
            ViewModel.Platforms = new SelectList(await _context.Platforms.ToListAsync(), "Id", "Name");
            ViewModel.Distributors = new SelectList(await _context.Distributors.ToListAsync(), "Id", "Name");
        }
    }
}
