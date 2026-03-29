namespace GameStore.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double? Price { get; set; }
        public int? SteamAppId { get; set; }
        public SteamApp? SteamApp { get; set; }

        public ICollection<Platform> Platforms { get; set; } = new List<Platform>();
        public ICollection<Distributor> Distributors { get; set; } = new List<Distributor>();
    }
}
