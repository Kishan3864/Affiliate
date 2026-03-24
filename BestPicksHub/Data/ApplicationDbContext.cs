using Microsoft.EntityFrameworkCore;
using BestPicksHub.Models;

namespace BestPicksHub.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<AffiliateProduct> AffiliateProducts => Set<AffiliateProduct>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<ArticleTag> ArticleTags => Set<ArticleTag>();
    public DbSet<Faq> Faqs => Set<Faq>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ArticleTag>()
            .HasKey(at => new { at.ArticleId, at.TagId });

        modelBuilder.Entity<ArticleTag>()
            .HasOne(at => at.Article)
            .WithMany(a => a.ArticleTags)
            .HasForeignKey(at => at.ArticleId);

        modelBuilder.Entity<ArticleTag>()
            .HasOne(at => at.Tag)
            .WithMany(t => t.ArticleTags)
            .HasForeignKey(at => at.TagId);

        modelBuilder.Entity<Article>()
            .HasIndex(a => a.Slug)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Slug)
            .IsUnique();

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Slug)
            .IsUnique();

        modelBuilder.Entity<Article>()
            .HasOne(a => a.Category)
            .WithMany(c => c.Articles)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AffiliateProduct>()
            .HasOne(p => p.Article)
            .WithMany(a => a.Products)
            .HasForeignKey(p => p.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Faq>()
            .HasOne(f => f.Article)
            .WithMany(a => a.Faqs)
            .HasForeignKey(f => f.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
