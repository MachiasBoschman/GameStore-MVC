using GameStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
            .HasOne(g => g.SteamApp)
            .WithOne(s => s.Game)
            .HasForeignKey<SteamApp>(s => s.GameId)
            .OnDelete(DeleteBehavior.Cascade);

            // Seed Data
            modelBuilder.Entity<Platform>().HasData(
                    new Platform { Id = 1, Name = "PC" },
                    new Platform { Id = 2, Name = "Playstation"},
                    new Platform { Id = 3, Name = "Xbox" }
                );
            modelBuilder.Entity<Distributor>().HasData(
                    new Distributor { Id = 1, Name = "Steam"},
                    new Distributor { Id = 2, Name = "Epic Games" },
                    new Distributor { Id = 3, Name = "Microsoft Store" },
                    new Distributor { Id = 4, Name = "Playstation" },
                    new Distributor { Id = 5, Name = "Xbox" }
                );
            modelBuilder.Entity<Game>().HasData(
                    new Game { Id = 1, Name = "Call Of Duty", Price = 50, SteamAppId = 1938090 }
                );
            modelBuilder.Entity<SteamApp>().HasData(
                    new SteamApp { Id = 1938090, GameId = 1 }
                );

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Game> Games { get; set; }
        public DbSet<SteamApp> SteamApps { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
    }
}