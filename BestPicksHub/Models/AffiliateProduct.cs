using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BestPicksHub.Models;

public class AffiliateProduct
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [MaxLength(200)]
    public string? ImageAlt { get; set; }

    [Required, MaxLength(500)]
    public string AffiliateUrl { get; set; } = string.Empty;

    public Platform Platform { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? OriginalPrice { get; set; }

    [MaxLength(50)]
    public string? Badge { get; set; }

    [MaxLength(500)]
    public string? ShortDescription { get; set; }

    [Column(TypeName = "decimal(2,1)")]
    public decimal Rating { get; set; }

    [MaxLength(1000)]
    public string? Pros { get; set; }

    [MaxLength(1000)]
    public string? Cons { get; set; }

    public int SortOrder { get; set; }

    public bool IsTopPick { get; set; }

    public int ArticleId { get; set; }

    [ForeignKey("ArticleId")]
    public Article? Article { get; set; }
}
