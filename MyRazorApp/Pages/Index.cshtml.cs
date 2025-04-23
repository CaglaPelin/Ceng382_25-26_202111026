/*
GPT Prompt:
Razor Pages projemde JSON olarak veri indirme butonu yapmak istiyorum. 
Kullanıcı sadece filtrelenmiş verileri görebilsin ve o sayfadaki veriyi JSON olarak indirebilsin. 
Ayrıca kullanıcı kolon başlıklarına tıklayarak sadece o kolonları seçebilmeli ve sadece seçilen kolonları JSON formatında dışa aktarabilmeli. 
Bu işlemleri sayfalama desteği ile birlikte nasıl yapabilirim?
Ayrıca güvenlik kontrolü yapmak istiyorum: Kullanıcının giriş yapıp yapmadığını Cookie ve Session üzerinden kontrol edip,
giriş yapılmadıysa kullanıcıyı login sayfasına yönlendirmek istiyorum.
*/








using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;
using MyRazorApp.Helpers;
using System.Text;

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

        [BindProperty]
        public List<int> SelectedClassIds { get; set; } = new();

        [BindProperty]
        public List<string> SelectedColumns { get; set; } = new();

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        private static int _nextId = 101;
        private static bool IsInitialized = false;
 

       public IActionResult OnGet()
        {
            var token = HttpContext.Session.GetString("token");
            var username = HttpContext.Session.GetString("username");
            var sessionId = HttpContext.Session.GetString("session_id");

            if (token == null || username == null || sessionId == null)
            {
                return RedirectToPage("/Login");
            }

            var cookieToken = Request.Cookies["token"];
            var cookieUser = Request.Cookies["username"];
            var cookieSessionId = Request.Cookies["session_id"];

            if (cookieToken != token || cookieUser != username || cookieSessionId != sessionId)
            {
                return RedirectToPage("/Login");
            }

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
            return Page();
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

            ApplyFilteringAndPaging();
            return RedirectToPage(new { FilterText, CurrentPage });
        }

        public IActionResult OnGetDownloadJson(string? filter, int currentPage)
        {
            var query = AllClasses.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
                query = query.Where(c => c.ClassName.Contains(filter, StringComparison.OrdinalIgnoreCase));

           
            int pageSize = PageSize;
            query = query.Skip((currentPage - 1) * pageSize).Take(pageSize);

            // JSON verisini oluşturma
            var json = JsonUtils.Instance.Serialize(query.ToList());
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", $"page-{currentPage}-classes.json");
        }

        public IActionResult OnPostExportSelectedJson()
        {
            var columns = Request.Form["SelectedColumns"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
            
            ApplyFilteringAndPaging();

            if (!columns.Any())
            {
                
                var json = JsonUtils.Instance.Serialize(FilteredClasses);
                var bytes = Encoding.UTF8.GetBytes(json);
                return File(bytes, "application/json", "all-columns.json");
            }

           
            var filteredData = FilteredClasses.Select(c =>
            {
                var result = new Dictionary<string, object>();
                foreach (var column in columns)
                {
                    var property = typeof(ClassInformationModel).GetProperty(column);
                    if (property != null)
                    {
                        var value = property.GetValue(c);
                      
                        result[column] = value ?? "N/A"; 
                    }
                }
                return result;
            }).ToList();

            var selectedJson = JsonUtils.Instance.Serialize(filteredData);
            var selectedBytes = Encoding.UTF8.GetBytes(selectedJson);

            return File(selectedBytes, "application/json", "selected-columns.json");
        }

        public IActionResult OnGetExportColumnJson(string column)
        {
            if (string.IsNullOrEmpty(column))
            {
                return BadRequest("Column name is required.");
            }

            ApplyFilteringAndPaging(); 

            var filteredData = FilteredClasses.Select(c =>
            {
                var result = new Dictionary<string, object>();
                var property = typeof(ClassInformationModel).GetProperty(column);
                if (property != null)
                {
                    result[column] = property.GetValue(c);
                }
                return result;
            }).ToList();

            var json = JsonUtils.Instance.Serialize(filteredData);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", $"{column}-data.json");
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                ApplyFilteringAndPaging();
                return Page();
            }

            NewClass.Id = _nextId++;
            AllClasses.Add(new ClassInformationModel
            {
                Id = NewClass.Id,
                ClassName = NewClass.ClassName,
                StudentCount = NewClass.StudentCount,
                Description = NewClass.Description
            });

            NewClass = new ClassInformationModel(); // Formu sıfırla
            ApplyFilteringAndPaging();
            return RedirectToPage(new { FilterText, CurrentPage });
        }

        public IActionResult OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                ApplyFilteringAndPaging();
                return Page();
            }

            var item = AllClasses.FirstOrDefault(c => c.Id == EditId);
            if (item != null)
            {
                item.ClassName = NewClass.ClassName;
                item.StudentCount = NewClass.StudentCount;
                item.Description = NewClass.Description;
            }

            EditId = 0; // Düzenleme modundan çık
            NewClass = new ClassInformationModel(); // Formu sıfırla
            ApplyFilteringAndPaging();
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
