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
            modelBuilder.Entity<Genre>().HasData(
                // Core genres
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Adventure" },
                new Genre { Id = 3, Name = "Casual" },
                new Genre { Id = 4, Name = "Indie" },
                new Genre { Id = 5, Name = "Massively Multiplayer" },
                new Genre { Id = 6, Name = "Racing" },
                new Genre { Id = 7, Name = "RPG" },
                new Genre { Id = 8, Name = "Simulation" },
                new Genre { Id = 9, Name = "Sports" },
                new Genre { Id = 10, Name = "Strategy" },
                new Genre { Id = 11, Name = "Free To Play" },

                // Popular sub-genres
                new Genre { Id = 12, Name = "First Person Shooter" },
                new Genre { Id = 13, Name = "Third Person Shooter" },
                new Genre { Id = 14, Name = "Battle Royale" },
                new Genre { Id = 15, Name = "MOBA" },
                new Genre { Id = 16, Name = "Roguelike" },
                new Genre { Id = 17, Name = "Metroidvania" },
                new Genre { Id = 18, Name = "Soulslike" },
                new Genre { Id = 19, Name = "Tower Defense" },
                new Genre { Id = 20, Name = "Turn Based Strategy" },
                new Genre { Id = 21, Name = "Real Time Strategy" },
                new Genre { Id = 22, Name = "Survival" },
                new Genre { Id = 23, Name = "Horror" },
                new Genre { Id = 24, Name = "Puzzle" },
                new Genre { Id = 25, Name = "Platformer" },
                new Genre { Id = 26, Name = "Fighting" },
                new Genre { Id = 27, Name = "Stealth" },
                new Genre { Id = 28, Name = "Open World" },
                new Genre { Id = 29, Name = "Visual Novel" },
                new Genre { Id = 30, Name = "City Builder" }
            );
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
        public DbSet<Genre> Genres { get; set; }
    }
}