using BestPicksHub.Data;
using Microsoft.EntityFrameworkCore;

namespace BestPicksHub.Routing;

public class SlugRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (httpContext == null) return false;
        if (!values.TryGetValue(routeKey, out var slugValue)) return false;

        var slug = slugValue?.ToString();
        if (string.IsNullOrEmpty(slug)) return false;

        // Exclude known routes
        var excludedPrefixes = new[] { "admin", "home", "category", "search", "sitemap.xml", "feed.xml", "robots.txt", "css", "js", "lib", "favicon.ico" };
        if (excludedPrefixes.Any(p => slug.Equals(p, StringComparison.OrdinalIgnoreCase) || slug.StartsWith(p + "/", StringComparison.OrdinalIgnoreCase)))
            return false;

        var dbContext = httpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
        return dbContext.Articles.Any(a => a.Slug == slug && a.IsPublished);
    }
}
