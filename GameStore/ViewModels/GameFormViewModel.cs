using GameStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GameStore.ViewModels
{
    public class GameFormViewModel
    {
        // Null for Create, populated for Edit
        public Game Game { get; set; } = null!;

        // The full list of available options to populate the dropdown
        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Platforms { get; set; } = new List<SelectListItem>();

        // The selected ids submitted from the form
        [Required(ErrorMessage = "Please select at least one genre.")]
        public List<int> GenreIds { get; set; } = new();

        [Required(ErrorMessage = "Please select at least one platform.")]
        public List<int> PlatformIds { get; set; } = new();
        public IFormFile? ImageFile { get; set; }
    }
}