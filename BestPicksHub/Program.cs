using Microsoft.EntityFrameworkCore;
using BestPicksHub.Data;
using BestPicksHub.Filters;
using BestPicksHub.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<NavCategoriesFilter>();
});
builder.Services.AddScoped<NavCategoriesFilter>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddResponseCaching();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

// Auto-migrate and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    SeedData.Initialize(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
    }
});
app.UseResponseCaching();
app.UseRouting();
app.UseAuthorization();

// Routes
app.MapControllerRoute(
    name: "sitemap",
    pattern: "sitemap.xml",
    defaults: new { controller = "Feed", action = "Sitemap" });

app.MapControllerRoute(
    name: "rssfeed",
    pattern: "feed.xml",
    defaults: new { controller = "Feed", action = "Rss" });

app.MapControllerRoute(
    name: "robots",
    pattern: "robots.txt",
    defaults: new { controller = "Feed", action = "Robots" });

app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{action=Dashboard}/{id?}",
    defaults: new { controller = "Admin" });

app.MapControllerRoute(
    name: "category",
    pattern: "category/{slug}",
    defaults: new { controller = "Category", action = "Index" });

app.MapControllerRoute(
    name: "search",
    pattern: "search",
    defaults: new { controller = "Home", action = "Search" });

app.MapControllerRoute(
    name: "about",
    pattern: "home/about",
    defaults: new { controller = "Home", action = "About" });

app.MapControllerRoute(
    name: "contact",
    pattern: "home/contact",
    defaults: new { controller = "Home", action = "Contact" });

app.MapControllerRoute(
    name: "privacy",
    pattern: "home/privacy",
    defaults: new { controller = "Home", action = "Privacy" });

app.MapControllerRoute(
    name: "terms",
    pattern: "home/terms",
    defaults: new { controller = "Home", action = "Terms" });

app.MapControllerRoute(
    name: "disclaimer",
    pattern: "home/disclaimer",
    defaults: new { controller = "Home", action = "Disclaimer" });

app.MapControllerRoute(
    name: "articlelist",
    pattern: "home/list",
    defaults: new { controller = "Article", action = "List" });

app.MapControllerRoute(
    name: "article",
    pattern: "{slug}",
    defaults: new { controller = "Article", action = "Detail" },
    constraints: new { slug = new SlugRouteConstraint() });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
