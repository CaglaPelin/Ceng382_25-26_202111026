using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRazorApp.Data;
using MyRazorApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRazorApp.Pages
{
    public class JsonViewModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public JsonViewModel(SchoolDbContext context)
        {
            _context = context;
        }

        public List<Class> FilteredClasses { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // FilteredClasses'ı veritabanından doldur
            FilteredClasses = await _context.Classes
                                            .Where(c => c.IsActive)
                                            .ToListAsync();

            return Page();
        }
    }
}
