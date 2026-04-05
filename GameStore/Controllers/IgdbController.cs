using GameStore.IgdbModels;
using GameStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    public class IgdbController : Controller
    {
        private readonly IIgdbService _igdbService;

        public IgdbController(IIgdbService igdbService)
        {
            _igdbService = igdbService;
        }

        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("No game name provided.");

            var igdbGame = await _igdbService.SearchGameAsync(name);

            if (igdbGame == null) return NotFound("No game found with that name.");
            var vm = await _igdbService.MapToViewModelAsync(igdbGame);

            return Json(vm);
        }
    }
}