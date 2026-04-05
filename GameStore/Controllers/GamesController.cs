using GameStore.Services;
using GameStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    public class GamesController : Controller
    {
        // Stores the context of the DB in the GamesController.
        private readonly IGameService _GameService;
        private readonly IIgdbService _igdbService;

        // GamesController automatically requests a connection with the DB on creation.
        public GamesController(IGameService GameService, IIgdbService igdbService)
        {
            _GameService = GameService;
            _igdbService = igdbService;
        }
        // Testing IgdbService game fetching through API
        // Games/TestIgdb
        public async Task<IActionResult> TestIgdb()
        {
            var result = await _igdbService.SearchGameAsync("Elden Ring");
            return Json(result);
        }
        // Simple getter Index page, includes all connections from Games
        // And wraps it as a model for /Games/Index
        public async Task<IActionResult> Index()
        {
            return View(await _GameService.GetAllAsync()); 
        }

        // Simple getter Overview page, includes one connection from Games
        // And wraps it as a model for /Games/Overview/id
        public async Task<IActionResult> Overview(int id)
        {
            return View(await _GameService.GetByIdAsync(id));
        }
        // This action populates the dropdowns needed for
        // the empty form of the Create view.
        public async Task<IActionResult> Create()
        {
            var vm = new GameFormViewModel();
            await _GameService.PopulateDropdownsAsync(vm);
            return View(vm);
        }
        // This handles the post functionality of the form
        [HttpPost]
        public async Task<IActionResult> Create(GameFormViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await _GameService.CreateAsync(vm);
                return RedirectToAction("Index");
            }
            await _GameService.PopulateDropdownsAsync(vm);
            return View(vm);
        }
        // Return the model of the requested DB entry to the view
        public async Task<IActionResult> Edit(int id)
        {
            // Try to find a game in the DB with given ID from url
            var game = await _GameService.GetByIdAsync(id);

            // If game is null, display error 404 response
            if (game == null) return NotFound();

            // Create FormViewModel for fluid interaction with forms
            var vm = new GameFormViewModel();
            _GameService.RetrieveRelationsAsync(game, vm);
            // Separate Lists for display in dropdowns
            await _GameService.PopulateDropdownsAsync(vm);
            return View(vm);
        }
        // Update the existing DB entry with the input fields from the view
        [HttpPost]
        public async Task<IActionResult> Edit(GameFormViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await _GameService.UpdateAsync(vm);
                return RedirectToAction("Index");

            }

            await _GameService.PopulateDropdownsAsync(vm);
            return View(vm);
        }
        // Return the model of the requested DB entry to the view
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _GameService.GetByIdAsync(id);
            return View(game);
        }
        // Confirm the deletion process, after pressing delete in the view.
        // Only delete if the DB entry still exists.
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deletionStatus = await _GameService.DeleteAsync(id);
            if (deletionStatus == false) return NotFound();

            return RedirectToAction("Index");
        }
    }
}
