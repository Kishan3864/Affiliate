using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BestPicksHub.Models;

public class Article
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(160)]
    public string? MetaDescription { get; set; }

    [MaxLength(100)]
    public string? FocusKeyword { get; set; }

    [MaxLength(500)]
    public string? Keywords { get; set; }

    public string? ContentHtml { get; set; }

    [MaxLength(500)]
    public string? Excerpt { get; set; }

    [MaxLength(500)]
    public string? FeaturedImageUrl { get; set; }

    [MaxLength(200)]
    public string? FeaturedImageAlt { get; set; }

    [MaxLength(100)]
    public string Author { get; set; } = "BestPicksHub Editorial Team";

    public ArticleType Type { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsPublished { get; set; }

    public bool IsFeatured { get; set; }

    public int ViewCount { get; set; }

    public int ReadTimeMinutes { get; set; }

    [Column(TypeName = "decimal(3,1)")]
    public decimal OverallRating { get; set; }

    public int RatingCount { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    public ICollection<AffiliateProduct> Products { get; set; } = new List<AffiliateProduct>();
    public ICollection<Faq> Faqs { get; set; } = new List<Faq>();
    public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
}
