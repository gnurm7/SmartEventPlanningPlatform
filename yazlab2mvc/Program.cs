using Microsoft.EntityFrameworkCore;
using yazlab2mvc.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with connection string
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// **Session'� ekle**
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum s�resi (30 dakika)
    options.Cookie.HttpOnly = true;                // G�venlik i�in HttpOnly cookie
    options.Cookie.IsEssential = true;             // GDPR uyumu i�in gerekli i�aretleme
});

// **JSON Ayarlar� i�in ekleme**
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // D�ng�sel referanslar i�in
    });

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// **Session middleware ekle**
app.UseSession(); // Burada session middleware'ini ekledik.

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Giris}/{id?}",
    defaults: new { controller = "Admin" }
);

//app.MapControllerRoute(
//    name: "etkinlik",
//    pattern: "Etkinlik/{action=EtkinlikOnerileri}/{userId?}",
//    defaults: new { controller = "Etkinlik" }
//);
app.MapControllerRoute(
    name: "etkinlik",
    pattern: "Kullanici/{action=EtkinlikOnerileri}/{userId?}",
    defaults: new { controller = "Etkinlik" }
);

app.Run();
