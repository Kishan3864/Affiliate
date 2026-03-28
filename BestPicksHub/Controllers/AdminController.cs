using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BestPicksHub.Data;
using BestPicksHub.Models;

namespace BestPicksHub.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Dashboard()
    {
        ViewBag.TotalArticles = await _context.Articles.CountAsync();
        ViewBag.PublishedArticles = await _context.Articles.CountAsync(a => a.IsPublished);
        ViewBag.TotalProducts = await _context.AffiliateProducts.CountAsync();
        ViewBag.TotalCategories = await _context.Categories.CountAsync();

        return View();
    }

    public async Task<IActionResult> Articles()
    {
        var articles = await _context.Articles
            .Include(a => a.Category)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return View(articles);
    }

    [HttpGet]
    public async Task<IActionResult> EditArticle(int? id)
    {
        ViewBag.Categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();

        if (id.HasValue)
        {
            var article = await _context.Articles
                .Include(a => a.Products.OrderBy(p => p.SortOrder))
                .Include(a => a.Faqs.OrderBy(f => f.SortOrder))
                .FirstOrDefaultAsync(a => a.Id == id.Value);

            if (article == null)
                return NotFound();

            return View(article);
        }

        return View(new Article());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditArticle(Article article)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            return View(article);
        }

        if (article.Id == 0)
        {
            article.CreatedAt = DateTime.UtcNow;
            article.UpdatedAt = DateTime.UtcNow;
            _context.Articles.Add(article);
        }
        else
        {
            var existing = await _context.Articles.FindAsync(article.Id);
            if (existing == null)
                return NotFound();

            existing.Title = article.Title;
            existing.Slug = article.Slug;
            existing.MetaDescription = article.MetaDescription;
            existing.FocusKeyword = article.FocusKeyword;
            existing.Keywords = article.Keywords;
            existing.ContentHtml = article.ContentHtml;
            existing.Excerpt = article.Excerpt;
            existing.FeaturedImageUrl = article.FeaturedImageUrl;
            existing.FeaturedImageAlt = article.FeaturedImageAlt;
            existing.Author = article.Author;
            existing.Type = article.Type;
            existing.IsPublished = article.IsPublished;
            existing.IsFeatured = article.IsFeatured;
            existing.ReadTimeMinutes = article.ReadTimeMinutes;
            existing.OverallRating = article.OverallRating;
            existing.RatingCount = article.RatingCount;
            existing.CategoryId = article.CategoryId;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Articles));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article != null)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Articles));
    }

    public async Task<IActionResult> Products()
    {
        var products = await _context.AffiliateProducts
            .Include(p => p.Article)
            .OrderBy(p => p.SortOrder)
            .ToListAsync();

        return View(products);
    }
}
