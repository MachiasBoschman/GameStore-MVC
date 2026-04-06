# 🎮 GameStore

A full-stack ASP.NET Core MVC web application styled after the classic Steam storefront. GameStore lets admins curate a catalogue of games — complete with cover art, descriptions, genres, and platform tags — while giving any visitor a clean, browsable library.

---

## ✨ Features

### Storefront
- **Game library** — horizontal card layout inspired by Steam's classic UI, with cover thumbnails, platform icons, genre tags, and pricing
- **Game overview page** — dedicated detail view per game with a sidebar news panel
- **Clickable cards** — the entire card navigates to the overview, keeping the UI intuitive

### Admin Panel
- **Role-based access** — only users with the `Admin` role can create, edit, or delete entries; action panels slide in on card hover for a polished UX
- **IGDB auto-fill** — type a game name, click the search icon, and the form auto-populates name, description, cover art, genres, and platforms by querying the [IGDB API](https://api-docs.igdb.com/) with a typewriter animation effect
- **Image upload** — upload a local cover image or use the IGDB-sourced URL; a live preview updates as you select a file
- **Custom delete dialog** — a retro Windows 2000-style confirmation dialog with beveled borders, gradient titlebar, and keyboard/Escape support

### Identity & Auth
- ASP.NET Core Identity with `IdentityUser` and role management (`Admin`)
- Login / Register / Logout flows
- `[Authorize(Roles = "Admin")]` guards on all mutation endpoints
   - User: user@gamestore.demo / Password: User@12345
   - Admin: admin@gamestore.demo / Password: Admin@12345
---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 MVC |
| ORM | Entity Framework Core 9 (Code-First) |
| Database | SQL Server |
| Auth | ASP.NET Core Identity |
| External API | IGDB (via Twitch OAuth2) |
| Front-end | Bootstrap 5, Bootstrap Icons, Select2 |
| Language | C# 13, Razor Views |

---

## 🏗️ Architecture

The project follows a clean layered architecture:

```
Controllers  ──▶  IGameService / IIgdbService  ──▶  DbContext (EF Core)
                        │
                   GameService
                   IgdbService
```

- **Service layer** (`GameService`, `IgdbService`) abstracts all database and HTTP logic away from controllers, keeping actions thin and testable
- **ViewModels** (`GameFormViewModel`) decouple form data from domain models, carrying dropdown lists, selected IDs, and file uploads cleanly through the request lifecycle
- **Options pattern** (`IgdbOptions`) binds IGDB credentials from configuration via `IOptions<T>`, keeping secrets out of code
- **Token caching** in `IgdbService` stores the Twitch OAuth2 access token in memory with expiry tracking, avoiding redundant auth calls

---

## 🗄️ Data Model

```
Game  ──< GameGenre >──  Genre
      ──< GamePlatform >──  Platform
      ──── SteamApp (1:1)
```

- Many-to-many relationships between `Game` ↔ `Genre` and `Game` ↔ `Platform` via EF Core's implicit join tables
- One-to-one `Game` ↔ `SteamApp` for Steam metadata
- `decimal(18,2)` pricing (migrated from `double` for financial accuracy)
- Nullable `ImagePath` supporting both uploaded local files and external URLs (e.g. IGDB CDN)

---

## 🎨 UI Design

The frontend is a hand-coded tribute to the **classic Steam (circa 2003–2010)** aesthetic:

- Muted olive/army green palette (`#3e4637`, `#272e22`) with golden price text (`#bfba4f`)
- Verdana typography, uppercased headings, tight letter-spacing
- **Responsive card layout** — gracefully degrades on mobile with scaled thumbnails and font sizes
- **Platform icon toggles** — PC/PlayStation/Xbox icons act as styled checkboxes that glow on selection
- **Auto-growing textarea** — CSS grid trick makes the description field expand with content rather than scroll
- **Card hover animations** — action panels slide in from the right using CSS transforms
- **Win2K delete dialog** — fully custom retro popup with beveled button borders, inset separator, and gradient titlebar

---

## 🔌 IGDB Integration

Game data is enriched via the [IGDB API](https://api-docs.igdb.com/):

1. Admin types a game name and clicks the lookup icon
2. A `fetch` call hits `/Igdb/Search?name=...`
3. `IgdbService` authenticates with Twitch OAuth2 (token cached), then queries IGDB for name, storyline, cover art, genres, and platforms
4. The server maps IGDB genres/platforms to the local database entries via fuzzy string matching
5. The client animates the results into the form fields with a typewriter effect

Cover images are automatically upgraded from IGDB's thumbnail size (`t_thumb`) to high-resolution (`t_cover_big`).

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB works fine for development)
- A [Twitch Developer](https://dev.twitch.tv/console) account for IGDB access

### Setup

1. **Clone the repo**
   ```bash
   git clone https://github.com/your-username/GameStore-MVC.git
   cd GameStore-MVC
   ```

2. **Configure secrets** — copy `appsettingsExample.json` to `appsettings.json` and fill in your values:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnectionString": "Server=(localdb)\\mssqllocaldb;Database=GameStore;Trusted_Connection=True;"
     },
     "Igdb": {
       "ClientId": "your-twitch-client-id",
       "ClientSecret": "your-twitch-client-secret"
     }
   }
   ```

3. **Apply migrations**
   ```bash
   dotnet ef database update OR Database-Update
   ```

4. **Run the app**
   ```bash
   dotnet watch
   ```

5. **Create an admin account** — register via `/Account/Register`, then grant the `Admin` role. The app seeds the role automatically on startup; you can assign it via the admin email set in `Program.cs` or directly through your database.

---

## 📁 Project Structure

```
GameStore/
├── Controllers/         # Thin MVC controllers
├── Data/                # DbContext (IdentityDbContext)
├── IgdbModels/          # DTOs for IGDB API responses
├── Migrations/          # EF Core migration history
├── Models/              # Domain entities (Game, Genre, Platform…)
├── Options/             # Strongly-typed config (IgdbOptions)
├── Services/            # Business logic (GameService, IgdbService)
├── ViewModels/          # Form view models
├── Views/               # Razor views
│   ├── Games/           # Index, Overview, Create, Edit
│   ├── Account/         # Login, Register
│   └── Shared/          # Layout, partials
└── wwwroot/             # Static assets (CSS, JS, images)
```

---

## 📸 Screenshots

> *Add screenshots of the game library, overview page, create/edit form, and the Win2K dialog here.*

---

## 🙏 Credits

UI design inspired by the iconic Steam storefront created by **Valve Corporation**. All original design concepts and stylistic choices are credited to their respective creators. This project is non-commercial and built purely for learning and portfolio purposes.

Game metadata and cover art sourced from the [IGDB API](https://api-docs.igdb.com/).