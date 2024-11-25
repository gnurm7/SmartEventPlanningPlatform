using yazlab2mvc.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabaný baðlantýsý yapýlandýrmasý
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer("Server=DESKTOP-PMBMQKM\\SQLEXPRESS;Database=yazlab2;Trusted_Connection=True;TrustServerCertificate=True;"));
//eðer  .addmvc() razor page  kulalnýýlýr
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
//dosya yollarýný buraya ekle
// eðer vievda dosya adi kullanýcýysa .cshtml ismi kullanýcontrollerolur
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
