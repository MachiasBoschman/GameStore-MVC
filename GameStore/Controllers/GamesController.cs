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
        private readonly GameStoreContext _context;

        public GamesController(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _context.Games
                .Include(g => g.SteamApp)
                .Include(g => g.Platforms)
                .Include(g => g.Distributors).ToListAsync();
            return View(games);
        }

        public async Task<IActionResult> Create()
        {
            var ViewModel = new GameFormViewModel();
            await PopulateDropdowns(ViewModel);
            return View(ViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(GameFormViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                ViewModel.Game.Platforms = await _context.Platforms
                    .Where(p => ViewModel.PlatformIds.Contains(p.Id))
                    .ToListAsync();
                ViewModel.Game.Distributors = await _context.Distributors
                    .Where(d => ViewModel.DistributorIds.Contains(d.Id))
                    .ToListAsync();

                _context.Games.Add(ViewModel.Game);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await PopulateDropdowns(ViewModel);
            return View(ViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == id);
            return View(game);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price, PlatformId")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Games.Update(game);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(game);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == id);
            return View(game);
        }
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

        private async Task PopulateDropdowns(GameFormViewModel ViewModel)
        {
            ViewModel.Platforms = new SelectList(await _context.Platforms.ToListAsync(), "Id", "Name");
            ViewModel.Distributors = new SelectList(await _context.Distributors.ToListAsync(), "Id", "Name");
        }
    }
}
