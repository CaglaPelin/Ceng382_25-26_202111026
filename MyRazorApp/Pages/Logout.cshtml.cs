/*
GPT Prompt:
Razor Pages projemde kullanıcı çıkış işlemini gerçekleştirmek istiyorum. 
Çıkış butonuna basıldığında Session içeriği temizlenmeli ve login sırasında ayarlanan tüm Cookie'ler silinmeli. 
Ardından kullanıcı login sayfasına yönlendirilmelidir.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyRazorApp.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Session temizle
            HttpContext.Session.Clear();

            // Çerezleri sil
            var cookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Delete("username", cookieOptions);
            Response.Cookies.Delete("token", cookieOptions);
            Response.Cookies.Delete("session_id", cookieOptions);

            // Login sayfasına yönlendir
            return RedirectToPage("/Login");
        }
    }
}


