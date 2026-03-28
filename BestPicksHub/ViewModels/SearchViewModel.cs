using BestPicksHub.Models;

namespace BestPicksHub.ViewModels;

public class SearchViewModel
{
    public string Query { get; set; } = string.Empty;
    public List<Article> Results { get; set; } = new();
    public int TotalResults { get; set; }
}
