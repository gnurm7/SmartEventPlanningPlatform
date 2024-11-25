using yazlab2mvc.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lant�s� yap�land�rmas�
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer("Server=DESKTOP-PMBMQKM\\SQLEXPRESS;Database=yazlab2;Trusted_Connection=True;TrustServerCertificate=True;"));
//e�er  .addmvc() razor page  kulaln��l�r
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
//dosya yollar�n� buraya ekle
// e�er vievda dosya adi kullan�c�ysa .cshtml ismi kullan�controllerolur
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
