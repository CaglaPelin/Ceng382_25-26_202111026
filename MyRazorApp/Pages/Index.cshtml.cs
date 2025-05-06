using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRazorApp.Models;
using MyRazorApp.Data;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Class NewClass { get; set; } = new();

        [BindProperty]
        public int EditId { get; set; }

        private readonly SchoolDbContext _context;

        public IndexModel(SchoolDbContext context)
        {
            _context = context;
        }

        public IList<Class> FilteredClasses { get; set; } = new List<Class>();

        [BindProperty(SupportsGet = true)]
        public string? FilterText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        [BindProperty]
        public List<string> SelectedColumns { get; set; } = new();

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            var username = HttpContext.Session.GetString("username");
            var sessionId = HttpContext.Session.GetString("session_id");

            if (token == null || username == null || sessionId == null)
                return RedirectToPage("/Login");

            var cookieToken = Request.Cookies["token"];
            var cookieUser = Request.Cookies["username"];
            var cookieSessionId = Request.Cookies["session_id"];

            if (cookieToken != token || cookieUser != username || cookieSessionId != sessionId)
                return RedirectToPage("/Login");

            await ApplyFilteringAndPagingAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostSaveToDatabaseAsync()
        {
            if (!ModelState.IsValid)
            {
                await ApplyFilteringAndPagingAsync();
                return Page();
            }

            try
            {
                if (EditId == 0)
                {
                    NewClass.IsActive = true;
                    _context.Classes.Add(NewClass);
                }
                else
                {
                    var existingClass = await _context.Classes.FindAsync(EditId);
                    if (existingClass == null)
                        return NotFound();

                    existingClass.Name = NewClass.Name;
                    existingClass.PersonCount = NewClass.PersonCount;
                    existingClass.Description = NewClass.Description;
                    _context.Classes.Update(existingClass);
                }

                await _context.SaveChangesAsync();
                return RedirectToPage();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while saving the data.");
                await ApplyFilteringAndPagingAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostLoadEditAsync(int id)
        {
            var classToEdit = await _context.Classes.FindAsync(id);
            if (classToEdit == null)
                return NotFound();

            EditId = classToEdit.Id;
            NewClass = new Class
            {
                Name = classToEdit.Name,
                PersonCount = classToEdit.PersonCount,
                Description = classToEdit.Description
            };

            await ApplyFilteringAndPagingAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var classToDelete = await _context.Classes.FindAsync(id);
            if (classToDelete == null)
                return NotFound();

            classToDelete.IsActive = false;
            _context.Classes.Update(classToDelete);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        private async Task ApplyFilteringAndPagingAsync()
        {
            var query = _context.Classes.Where(c => c.IsActive);

            if (!string.IsNullOrEmpty(FilterText))
                query = query.Where(c => c.Name.Contains(FilterText));

            TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)PageSize);

            FilteredClasses = await query
                .OrderBy(c => c.Id)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
