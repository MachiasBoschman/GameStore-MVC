using GameStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.ViewModels
{
    public class GameFormViewModel
    {
        // Null for Create, populated for Edit
        public Game Game { get; set; } = null!;

        // The full list of available options to populate the dropdown
        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Platforms { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Distributors { get; set; } = new List<SelectListItem>();

        // The selected ids submitted from the form
        public List<int> GenreIds { get; set; } = new List<int>();
        public List<int> PlatformIds { get; set; } = new List<int>();
        public List<int> DistributorIds { get; set; } = new List<int>();
    }
}