using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BestPicksHub.Data;
using BestPicksHub.Models;
using BestPicksHub.ViewModels;

namespace BestPicksHub.Controllers;

public class ArticleController : Controller
{
    private readonly ApplicationDbContext _context;
    private const int PageSize = 12;

    public ArticleController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Detail(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return NotFound();

        var article = await _context.Articles
            .Include(a => a.Products.OrderBy(p => p.SortOrder))
            .Include(a => a.Faqs.OrderBy(f => f.SortOrder))
            .Include(a => a.Category)
            .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
            .FirstOrDefaultAsync(a => a.Slug == slug && a.IsPublished);

        if (article == null)
            return NotFound();

        article.ViewCount++;
        await _context.SaveChangesAsync();

        var relatedArticles = await _context.Articles
            .Where(a => a.CategoryId == article.CategoryId && a.Id != article.Id && a.IsPublished)
            .Include(a => a.Category)
            .OrderByDescending(a => a.CreatedAt)
            .Take(4)
            .ToListAsync();

        var popularArticles = await _context.Articles
            .Where(a => a.IsPublished)
            .Include(a => a.Category)
            .OrderByDescending(a => a.ViewCount)
            .Take(5)
            .ToListAsync();

        var allCategories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();

        var viewModel = new ArticleDetailViewModel
        {
            Article = article,
            RelatedArticles = relatedArticles,
            AllCategories = allCategories,
            PopularArticles = popularArticles
        };

        return View(viewModel);
    }

    public async Task<IActionResult> List(int page = 1, string? type = null)
    {
        if (page < 1) page = 1;

        var query = _context.Articles
            .Where(a => a.IsPublished)
            .Include(a => a.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(type) && Enum.TryParse<ArticleType>(type, true, out var articleType))
        {
            query = query.Where(a => a.Type == articleType);
        }

        var totalArticles = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalArticles / (double)PageSize);

        var articles = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        var viewModel = new ArticleListViewModel
        {
            Articles = articles,
            CurrentPage = page,
            TotalPages = totalPages,
            ArticleType = type
        };

        return View(viewModel);
    }
}
