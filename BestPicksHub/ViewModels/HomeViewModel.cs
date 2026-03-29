using BestPicksHub.Models;

namespace BestPicksHub.ViewModels;

public class HomeViewModel
{
    public List<Article> FeaturedArticles { get; set; } = new();
    public List<Article> LatestArticles { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<AffiliateProduct> TopPicks { get; set; } = new();
}
