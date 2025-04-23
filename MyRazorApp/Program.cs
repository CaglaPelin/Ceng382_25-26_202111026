/*
GPT Prompt:
Razor Pages projemde Session desteğini aktif hale getirmek istiyorum. 
Session kullanımı için gerekli olan servis tanımı ve middleware ekleme işlemleri Program.cs dosyasına nasıl yerleştirilmelidir? 
Sıralama neden önemlidir?
*/

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddSession();

var app = builder.Build();
app.UseSession();
app.MapRazorPages();

app.Run();
