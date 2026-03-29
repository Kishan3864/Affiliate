using BestPicksHub.Models;

namespace BestPicksHub.ViewModels;

public class ArticleDetailViewModel
{
    public Article Article { get; set; } = null!;
    public List<Article> RelatedArticles { get; set; } = new();
    public List<Category> AllCategories { get; set; } = new();
    public List<Article> PopularArticles { get; set; } = new();
}

public class ArticleListViewModel
{
    public List<Article> Articles { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public string? ArticleType { get; set; }
}
