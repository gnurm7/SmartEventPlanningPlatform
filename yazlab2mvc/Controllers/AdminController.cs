using Microsoft.AspNetCore.Mvc;
using yazlab2mvc.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    private readonly Context _context;

    public AdminController(Context context)
    {
        _context = context;
    }

   
    public IActionResult Giris()
    {
        return View();  
    }

    
    [HttpPost]
    public IActionResult Giris(string KullaniciAdi, string Sifre)
    {
        
        if (KullaniciAdi == AdminBilgileri.KullaniciAdi && Sifre == AdminBilgileri.Sifre)
        {
            
            return RedirectToAction("Index", "Admin");
        }
        else
        {
            // Giriş başarısız, hata mesajı göster
            ViewBag.ErrorMessage = "Kullanıcı adı veya şifre hatalı!";
            return View(); 
        }
    }

    
    public IActionResult Index()
    {
        var etkinlikler = _context.Etkinlikler.ToList();
        return View(etkinlikler); 
    }


    
    public IActionResult KullaniciListele()
    {
        var kullanicilar = _context.Kullanicilar.ToList(); 
        return View(kullanicilar); 
    }

    
    public IActionResult KullaniciDetay(int id)
    {


        var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == id); 
        if (kullanici == null)
        {
            return NotFound(); 
        }
        return View(kullanici); 
    }

    [HttpPost]
    public IActionResult KullaniciSil(int id)
    {
        var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == id);

        if (kullanici != null)
        {
           
            var mesajlar = _context.Mesajlar.Where(m => m.AliciID == id).ToList();
            if (mesajlar.Any())
            {
                _context.Mesajlar.RemoveRange(mesajlar); 
            }

           
            _context.Kullanicilar.Remove(kullanici);

            
            _context.SaveChanges();
        }

       
        return RedirectToAction("KullaniciListele");
    }


    
    public IActionResult EtkinlikListele(string durum)
    {
        List<Etkinlikler> etkinlikler;

        if (string.IsNullOrEmpty(durum))
        {
            etkinlikler = _context.Etkinlikler.ToList();  
        }
        else
        {
           
            etkinlikler = _context.Etkinlikler.Where(e => e.EtkinlikDurumu == durum).ToList();
        }

        return View(etkinlikler); 
    }

    
    [HttpGet]
    public IActionResult EtkinlikGuncelle(int id)
    {
        var etkinlik = _context.Etkinlikler.FirstOrDefault(e => e.ID == id);
        if (etkinlik == null)
        {
            return NotFound();
        }
        return View(etkinlik);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EtkinlikGuncelle(Etkinlikler etkinlik)
    {
      
        var mevcutEtkinlik = _context.Etkinlikler.FirstOrDefault(e => e.ID == etkinlik.ID);

        if (mevcutEtkinlik == null)
        {
            ViewBag.m1 = "Etkinlik Not Found!";
            TempData["SuccessMessage"] = ViewBag.m1;
            return View(etkinlik);
        }

        
        mevcutEtkinlik.EtkinlikAdi = etkinlik.EtkinlikAdi;
        mevcutEtkinlik.Aciklama = etkinlik.Aciklama;
        mevcutEtkinlik.Tarih = etkinlik.Tarih;
        mevcutEtkinlik.Saat = etkinlik.Saat;
        mevcutEtkinlik.EtkinlikSuresi = etkinlik.EtkinlikSuresi;
        mevcutEtkinlik.OlusturanKullaniciID = mevcutEtkinlik.OlusturanKullaniciID;
        mevcutEtkinlik.Konum = etkinlik.Konum;
        mevcutEtkinlik.Kategori = etkinlik.Kategori;
        
        _context.Update(mevcutEtkinlik);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Etkinlik başarıyla güncellendi.";
        return RedirectToAction("EtkinlikListele");
    }




   
    [HttpPost]
    public IActionResult EtkinlikOnayla(int id)
    {
        var etkinlik = _context.Etkinlikler.FirstOrDefault(e => e.ID == id);

        if (etkinlik != null)
        {
            etkinlik.EtkinlikDurumu = "Onaylı"; 
            _context.SaveChanges();
        }

        return RedirectToAction("EtkinlikListele", new { durum = "Onaylı" }); 
    }

   
    [HttpPost]
    public IActionResult EtkinlikRed(int id)
    {
        var etkinlik = _context.Etkinlikler.FirstOrDefault(e => e.ID == id);

        if (etkinlik != null)
        {
            etkinlik.EtkinlikDurumu = "Reddedildi"; 
            _context.SaveChanges(); 
        }

        return RedirectToAction("EtkinlikListele", new { durum = "Reddedildi" }); 
    }


    
    [HttpPost]
    public IActionResult EtkinlikSil(int id)
    {
        var etkinlik = _context.Etkinlikler.FirstOrDefault(e => e.ID == id);

        if (etkinlik != null)
        {
            
            var katilimcilar = _context.Katilimcilar.Where(k => k.EtkinlikID == id).ToList();
            if (katilimcilar.Any())
            {
                _context.Katilimcilar.RemoveRange(katilimcilar); 
            }

            
            _context.Etkinlikler.Remove(etkinlik);
            _context.SaveChanges(); 
        }

        return RedirectToAction("EtkinlikListele"); 
    }

    public IActionResult KatilimciListele()
    {
        var katilimcilar = _context.Katilimcilar
                                    .Include(k => k.Kullanici)     
                                    .Include(k => k.Etkinlik)     
                                    .ToList();                    

        return View(katilimcilar); 
    }






    // Katılımcıyı sil
    [HttpPost]
    public IActionResult KatilimciSil(int kullaniciID, int etkinlikID)
    {
        var katilimci = _context.Katilimcilar
                                .FirstOrDefault(k => k.KullaniciID == kullaniciID && k.EtkinlikID == etkinlikID);

        if (katilimci != null)
        {
            _context.Katilimcilar.Remove(katilimci); // Katılımcıyı sil
            _context.SaveChanges(); // Değişiklikleri kaydet
        }

        return RedirectToAction("KatilimciListele"); // Katılımcılar listesine yönlendir
    }
}
