using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;
using MyRazorApp.Helpers;

namespace MyRazorApp.Pages
{
    public class JsonViewModel : PageModel
    {
        public string JsonOutput { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string? FilterText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public void OnGet()
        {
            var pageSize = 10;
            var query = IndexModel.AllClasses.AsQueryable();

            if (!string.IsNullOrEmpty(FilterText))
                query = query.Where(c => c.ClassName.Contains(FilterText, StringComparison.OrdinalIgnoreCase));

            var paged = query
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            JsonOutput = JsonUtils.Instance.Serialize(paged);
        }
    }
}
