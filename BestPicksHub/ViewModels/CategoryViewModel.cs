using BestPicksHub.Models;

namespace BestPicksHub.ViewModels;

public class CategoryViewModel
{
    public Category Category { get; set; } = null!;
    public List<Article> Articles { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
}
