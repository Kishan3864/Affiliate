using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using BestPicksHub.Data;

namespace BestPicksHub.Filters;

public class NavCategoriesFilter : IAsyncActionFilter
{
    private readonly ApplicationDbContext _context;

    public NavCategoriesFilter(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Controller is Controller controller)
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            controller.ViewData["NavCategories"] = categories;
        }

        await next();
    }
}
