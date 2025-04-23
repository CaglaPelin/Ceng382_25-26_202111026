/*
GPT Prompt:
Yerleşik kimlik doğrulama kullanmadan Razor Pages projemde bir giriş işlemi gerçekleştirmek istiyorum. 
Kullanıcı bilgileri JSON dosyasından okunmalı ve eşleşme varsa Session + Cookie oluşturulmalı. 
Session'da username, token ve session_id saklanmalı. Cookie'ler 30 dakika süreli olmalı ve güvenli ayarlarla oluşturulmalı. 
Giriş başarılıysa kullanıcı tablo sayfasına yönlendirilmeli. Giriş başarısızsa hata mesajı gösterilmeli.
*/



using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;
using System.Text.Json;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string Message { get; set; }

    public IActionResult OnPost()
    {
        var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/users.json");
        var users = JsonSerializer.Deserialize<List<User>>(System.IO.File.ReadAllText(jsonPath));

        var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password && u.IsActive);

        if (user != null)
        {
            var token = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("token", token);
            HttpContext.Session.SetString("username", Username);
            HttpContext.Session.SetString("session_id", HttpContext.Session.Id);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("token", token, cookieOptions);
            Response.Cookies.Append("username", Username, cookieOptions);
            Response.Cookies.Append("session_id", HttpContext.Session.Id, cookieOptions);

            return RedirectToPage("/Index"); // tablo sayfanızın adı
        }

        Message = "Kullanıcı adı veya şifre yanlış!";
        return Page();
    }
}
