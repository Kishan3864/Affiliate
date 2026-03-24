using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BestPicksHub.Models;

public class Faq
{
    public int Id { get; set; }

    [Required, MaxLength(500)]
    public string Question { get; set; } = string.Empty;

    [Required]
    public string Answer { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public int ArticleId { get; set; }

    [ForeignKey("ArticleId")]
    public Article? Article { get; set; }
}
