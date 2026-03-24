using System.ComponentModel.DataAnnotations;

namespace BestPicksHub.Models;

public class Tag
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Slug { get; set; } = string.Empty;

    public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
}
