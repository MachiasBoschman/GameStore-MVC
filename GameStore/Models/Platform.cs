namespace GameStore.Models
{
    public class Platform
    {
        public int Id { get; set; }
        public String Name { get; set; } = null!;
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
