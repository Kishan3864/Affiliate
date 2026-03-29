using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BestPicksHub.Data;
using BestPicksHub.ViewModels;

namespace BestPicksHub.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public HomeController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            FeaturedArticles = await _context.Articles
                .Where(a => a.IsFeatured && a.IsPublished)
                .Include(a => a.Category)
                .OrderByDescending(a => a.CreatedAt)
                .Take(4)
                .ToListAsync(),

            LatestArticles = await _context.Articles
                .Where(a => a.IsPublished)
                .Include(a => a.Category)
                .OrderByDescending(a => a.CreatedAt)
                .Take(8)
                .ToListAsync(),

            Categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .ToListAsync(),

            TopPicks = await _context.AffiliateProducts
                .Where(p => p.IsTopPick)
                .Include(p => p.Article)
                .OrderBy(p => p.SortOrder)
                .Take(6)
                .ToListAsync()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Search(string q)
    {
        var query = q?.Trim() ?? string.Empty;

        var viewModel = new SearchViewModel
        {
            Query = query
        };

        if (!string.IsNullOrWhiteSpace(query))
        {
            var results = await _context.Articles
                .Where(a => a.IsPublished &&
                    (a.Title.Contains(query) || a.ContentHtml!.Contains(query)))
                .Include(a => a.Category)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            viewModel.Results = results;
            viewModel.TotalResults = results.Count;
        }

        return View(viewModel);
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Terms()
    {
        return View();
    }

    public IActionResult Disclaimer()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? statusCode = null)
    {
        if (statusCode.HasValue)
        {
            ViewBag.StatusCode = statusCode.Value;
            ViewBag.Message = statusCode.Value switch
            {
                404 => "The page you're looking for could not be found.",
                500 => "An internal server error occurred.",
                403 => "You don't have permission to access this resource.",
                _ => "An unexpected error occurred."
            };
        }
        else
        {
            ViewBag.StatusCode = 500;
            ViewBag.Message = "An unexpected error occurred.";
        }

        return View();
    }
}
