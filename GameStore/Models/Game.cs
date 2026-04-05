using GameStore.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class Game
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Price is required.")]
        [Range(0, 200, ErrorMessage = "Price must be between 0 and 200.")]
        public double Price { get; set; }
        public string? Description { get; set; }
        public int? SteamAppId { get; set; }
        public SteamApp? SteamApp { get; set; }
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public ICollection<Platform> Platforms { get; set; } = new List<Platform>();
        public ICollection<Distributor> Distributors { get; set; } = new List<Distributor>();
        public string? ImagePath { get; set; }
    }
}
