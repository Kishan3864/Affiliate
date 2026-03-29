using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BestPicksHub.Data;
using BestPicksHub.ViewModels;

namespace BestPicksHub.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    private const int PageSize = 12;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string slug, int page = 1)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return NotFound();

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Slug == slug);

        if (category == null)
            return NotFound();

        if (page < 1) page = 1;

        var query = _context.Articles
            .Where(a => a.CategoryId == category.Id && a.IsPublished)
            .Include(a => a.Category);

        var totalArticles = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalArticles / (double)PageSize);

        var articles = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        var viewModel = new CategoryViewModel
        {
            Category = category,
            Articles = articles,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(viewModel);
    }
}
