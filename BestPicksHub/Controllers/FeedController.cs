using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BestPicksHub.Data;

namespace BestPicksHub.Controllers;

public class FeedController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public FeedController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<IActionResult> Sitemap()
    {
        var siteUrl = _configuration["SiteSettings:SiteUrl"]?.TrimEnd('/') ?? "https://localhost";

        var articles = await _context.Articles
            .Where(a => a.IsPublished)
            .OrderByDescending(a => a.UpdatedAt)
            .Select(a => new { a.Slug, a.UpdatedAt })
            .ToListAsync();

        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .Select(c => new { c.Slug })
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        // Static pages
        var staticPages = new[] { "", "/home/about", "/home/contact", "/home/privacy", "/home/terms", "/home/disclaimer" };
        foreach (var page in staticPages)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{siteUrl}{page}</loc>");
            sb.AppendLine("    <changefreq>monthly</changefreq>");
            sb.AppendLine($"    <priority>{(page == "" ? "1.0" : "0.3")}</priority>");
            sb.AppendLine("  </url>");
        }

        // Category pages
        foreach (var category in categories)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{siteUrl}/category/{category.Slug}</loc>");
            sb.AppendLine("    <changefreq>weekly</changefreq>");
            sb.AppendLine("    <priority>0.7</priority>");
            sb.AppendLine("  </url>");
        }

        // Article pages
        foreach (var article in articles)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{siteUrl}/{article.Slug}</loc>");
            sb.AppendLine($"    <lastmod>{article.UpdatedAt:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("    <changefreq>weekly</changefreq>");
            sb.AppendLine("    <priority>0.8</priority>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");

        return Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }

    public async Task<IActionResult> Rss()
    {
        var siteUrl = _configuration["SiteSettings:SiteUrl"]?.TrimEnd('/') ?? "https://localhost";

        var articles = await _context.Articles
            .Where(a => a.IsPublished)
            .Include(a => a.Category)
            .OrderByDescending(a => a.CreatedAt)
            .Take(20)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<rss version=\"2.0\" xmlns:atom=\"http://www.w3.org/2005/Atom\">");
        sb.AppendLine("  <channel>");
        sb.AppendLine("    <title>BestPicksHub</title>");
        sb.AppendLine($"    <link>{siteUrl}</link>");
        sb.AppendLine("    <description>Expert reviews, comparisons, and buying guides to help you find the best products.</description>");
        sb.AppendLine("    <language>en-us</language>");
        sb.AppendLine($"    <lastBuildDate>{DateTime.UtcNow:R}</lastBuildDate>");
        sb.AppendLine($"    <atom:link href=\"{siteUrl}/feed/rss\" rel=\"self\" type=\"application/rss+xml\" />");

        foreach (var article in articles)
        {
            sb.AppendLine("    <item>");
            sb.AppendLine($"      <title><![CDATA[{article.Title}]]></title>");
            sb.AppendLine($"      <link>{siteUrl}/article/{article.Slug}</link>");
            sb.AppendLine($"      <guid isPermaLink=\"true\">{siteUrl}/article/{article.Slug}</guid>");
            sb.AppendLine($"      <description><![CDATA[{article.Excerpt ?? article.MetaDescription ?? ""}]]></description>");
            sb.AppendLine($"      <author>{article.Author}</author>");
            if (article.Category != null)
            {
                sb.AppendLine($"      <category>{article.Category.Name}</category>");
            }
            sb.AppendLine($"      <pubDate>{article.CreatedAt:R}</pubDate>");
            sb.AppendLine("    </item>");
        }

        sb.AppendLine("  </channel>");
        sb.AppendLine("</rss>");

        return Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }

    public IActionResult Robots()
    {
        var siteUrl = _configuration["SiteSettings:SiteUrl"]?.TrimEnd('/') ?? "https://localhost";

        var sb = new StringBuilder();
        sb.AppendLine("User-agent: *");
        sb.AppendLine("Allow: /");
        sb.AppendLine();
        sb.AppendLine($"Sitemap: {siteUrl}/sitemap.xml");

        return Content(sb.ToString(), "text/plain", Encoding.UTF8);
    }
}
