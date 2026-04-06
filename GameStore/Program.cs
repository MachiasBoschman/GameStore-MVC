using GameStore.Data;
using GameStore.Options;
using GameStore.Services;
using Microsoft.AspNetCore.Identity;
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
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<GameStoreContext>()
    .AddDefaultTokenProviders();
// Bind IGDB options
builder.Services.Configure<IgdbOptions>(builder.Configuration.GetSection("Igdb"));

// Register HttpClients
builder.Services.AddHttpClient<IIgdbService, IgdbService>();

builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<GameStoreContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    // 1. Automatically create the database and apply migrations
    await context.Database.MigrateAsync();

    // 2. Seed Roles
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    // 3. Seed Admin User
    var adminEmail = "admin@gmail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        await userManager.CreateAsync(adminUser, "Admin@12345"); // Use a secure default
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

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
app.UseAuthentication();
app.UseAuthorization();
// Serves custom front-end code from the wwwroot folder
// (CSS, JS, images)
app.MapStaticAssets();
// Defines the expected URL pattern
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Games}/{action=Index}/{id?}")
    .WithStaticAssets();

// Starts listening to HTTP requests
app.Run();