using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models
{
    public class SteamApp
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public Game Game { get; set; } = null!;
    }
}
