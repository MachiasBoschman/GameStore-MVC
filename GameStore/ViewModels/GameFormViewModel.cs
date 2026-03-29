using GameStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.ViewModels
{
    public class GameFormViewModel
    {
        // Null for Create, populated for Edit
        public Game Game { get; set; } = null!;

        // Dropdown options to display
        public IEnumerable<SelectListItem> Platforms { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Distributors { get; set; } = new List<SelectListItem>();

        // Selected values
        public int[] PlatformIds { get; set; } = Array.Empty<int>();
        public int[] DistributorIds { get; set; } = Array.Empty<int>();
    }
}