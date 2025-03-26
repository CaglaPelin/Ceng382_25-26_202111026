// Prompt (GPT):
// "How can I handle Add, Edit, and Delete operations on a Razor Pages project without using JavaScript? 
//Provide example methods using IActionResult in C#, and ensure form validation with attributes like [Required] and [Range]."



using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        public static List<ClassInformationModel> ClassList { get; set; } = new();
        private static int _currentId = 1;

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        [BindProperty]
        public int EditId { get; set; }

        public string ButtonText { get; set; } = "Add Class";

        public void OnGet()
        {
            ButtonText = "Add Class";
        }

        public IActionResult OnPostSubmit()
        {
            if (!ModelState.IsValid)
                return Page();

            if (EditId == 0) // ADD MODE
            {
                NewClass.Id = _currentId++;
                ClassList.Add(NewClass);
            }
            else // UPDATE MODE
            {
                var item = ClassList.FirstOrDefault(x => x.Id == EditId);
                if (item != null)
                {
                    item.ClassName = NewClass.ClassName;
                    item.StudentCount = NewClass.StudentCount;
                    item.Description = NewClass.Description;
                }
            }

            return RedirectToPage();
        }

        public IActionResult OnPostLoadEdit(int id)
        {
            var item = ClassList.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                NewClass = new ClassInformationModel
                {
                    ClassName = item.ClassName,
                    StudentCount = item.StudentCount,
                    Description = item.Description
                };
                EditId = item.Id;
                ButtonText = "Update Class";
            }
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(x => x.Id == id);
            if (item != null)
                ClassList.Remove(item);

            return RedirectToPage();
        }
    }
}
