using GameStore.Data;
using GameStore.Options;
using GameStore.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Registers the necessary info about Controller mapping and SQL connection
// before running the app.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GameStoreContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnectionString"))
);
// Bind IGDB options
builder.Services.Configure<IgdbOptions>(builder.Configuration.GetSection("Igdb"));

// Register HttpClients
builder.Services.AddHttpClient<IIgdbService, IgdbService>();

builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

// Redirects towards error page to hide debug info from users.
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Http automatically redirects to Https
app.UseHttpsRedirection();
// Enable routing logic, converts URLs to corresponding controllers/actions.
app.UseRouting();
// Allows [Authorize] attributes to work
app.UseAuthorization();
// Serves custom front-end code from the wwwroot folder
// (CSS, JS, images)
app.MapStaticAssets();
// Defines the expected URL pattern
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Starts listening to HTTP requests
app.Run();