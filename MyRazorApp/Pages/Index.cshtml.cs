// GPT Prompt:
// I am developing a Razor Pages application for my CENG 382 assignment (Week 6). 
// I need to implement full backend logic using C# only — including class creation, editing, deletion, filtering, and pagination. 
// JavaScript or any frontend-based filtering is not allowed. 
// The pagination and filtering logic must be handled via LINQ inside the OnGet method.
// Additionally, I want to generate 100 synthetic data entries on the first page load for testing pagination properly.
// The application must update the table after every operation, and the form should switch between Add and Update mode based on user action.
// Implement backend-side pagination using C# and LINQ in a Razor Pages project.
// The pagination should work on filtered data, and the logic must be placed inside OnGet().
// The current page and filter text should be bound via query parameters using [BindProperty(SupportsGet = true)].
// Use LINQ’s Skip and Take methods to divide the data into pages.
// Also, compute the total page count and expose it to the frontend for rendering numbered buttons.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        public static List<ClassInformationModel> AllClasses { get; set; } = new();
        public List<ClassInformationModel> FilteredClasses { get; set; } = new();

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        [BindProperty]
        public int EditId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FilterText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        private static int _nextId = 101;

     private static bool IsInitialized = false;

public void OnGet()
{
    if (!IsInitialized)
    {
        for (int i = 1; i <= 100; i++)
        {
            AllClasses.Add(new ClassInformationModel
            {
                Id = i,
                ClassName = $"Class {i}",
                StudentCount = 20 + i,
                Description = $"This is class number {i}."
            });
        }

        IsInitialized = true;
    }

    ApplyFilteringAndPaging();
}


        public IActionResult OnPostSubmit()
        {
            if (!ModelState.IsValid)
            {
                ApplyFilteringAndPaging();
                return Page();
            }

            if (EditId == 0)
            {
                NewClass.Id = _nextId++;
                AllClasses.Add(NewClass);
            }
            else
            {
                var item = AllClasses.FirstOrDefault(c => c.Id == EditId);
                if (item != null)
                {
                    item.ClassName = NewClass.ClassName;
                    item.StudentCount = NewClass.StudentCount;
                    item.Description = NewClass.Description;
                }
            }

            return RedirectToPage(new { FilterText, CurrentPage });
        }

        public IActionResult OnPostLoadEdit(int id)
        {
            var item = AllClasses.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                NewClass = new ClassInformationModel
                {
                    ClassName = item.ClassName,
                    StudentCount = item.StudentCount,
                    Description = item.Description
                };
                EditId = item.Id;
            }

            ApplyFilteringAndPaging();
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = AllClasses.FirstOrDefault(c => c.Id == id);
            if (item != null)
                AllClasses.Remove(item);

            return RedirectToPage(new { FilterText, CurrentPage });
        }

        private void ApplyFilteringAndPaging()
        {
            var query = AllClasses.AsQueryable();

            if (!string.IsNullOrEmpty(FilterText))
                query = query.Where(c => c.ClassName.Contains(FilterText, StringComparison.OrdinalIgnoreCase));

            TotalPages = (int)Math.Ceiling(query.Count() / (double)PageSize);
            query = query.Skip((CurrentPage - 1) * PageSize).Take(PageSize);

            FilteredClasses = query.ToList();
        }
    }
}
