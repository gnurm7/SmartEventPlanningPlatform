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

    // Admin giriş sayfasını render et
    public IActionResult Giris()
    {
        return View();  // /Views/Admin/Giris.cshtml'yi render eder
    }

    // Giriş bilgileri gönderildiğinde çalışacak post action
    [HttpPost]
    public IActionResult Giris(string KullaniciAdi, string Sifre)
    {
        // Admin bilgilerini kontrol et
        if (KullaniciAdi == AdminBilgileri.KullaniciAdi && Sifre == AdminBilgileri.Sifre)
        {
            // Giriş başarılı, admin index sayfasına yönlendir
            return RedirectToAction("Index", "Admin");
        }
        else
        {
            // Giriş başarısız, hata mesajı göster
            ViewBag.ErrorMessage = "Kullanıcı adı veya şifre hatalı!";
            return View(); // Giriş sayfasına hata mesajı ile birlikte geri dön
        }
    }

    // Admin panelinin ana sayfası (admin index)
    public IActionResult Index()
    {
        var etkinlikler = _context.Etkinlikler.ToList(); // Etkinlikleri listele
        return View(etkinlikler); // Listeyi view'a gönder
    }


    // Kullanıcıları listele
    public IActionResult KullaniciListele()
    {
        var kullanicilar = _context.Kullanicilar.ToList(); // Veritabanından kullanıcıları al
        return View(kullanicilar); // Kullanıcıları listelemek için view'a gönder
    }

    // Kullanıcı detaylarını görüntüle
    public IActionResult KullaniciDetay(int id)
    {


        var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == id); // Kullanıcıyı ID'ye göre al
        if (kullanici == null)
        {
            return NotFound(); // Kullanıcı bulunamadıysa hata döndür
        }
        return View(kullanici); // Kullanıcıyı view'a gönder
    }

    // Kullanıcıyı sil
    [HttpPost]
    public IActionResult KullaniciSil(int id)
    {
        var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == id);
        if (kullanici != null)
        {
            _context.Kullanicilar.Remove(kullanici); // Kullanıcıyı veritabanından sil
            _context.SaveChanges(); // Değişiklikleri kaydet
        }
        return RedirectToAction("KullaniciListele"); // Kullanıcılar listesine yönlendir
    }

    // Etkinlikleri listele
    public IActionResult EtkinlikListele(string durum)
    {
        List<Etkinlikler> etkinlikler;

        if (string.IsNullOrEmpty(durum))
        {
            etkinlikler = _context.Etkinlikler.ToList();  // Durum verilmemişse tüm etkinlikleri al
        }
        else
        {
            // Belirtilen duruma göre etkinlikleri filtrele
            etkinlikler = _context.Etkinlikler.Where(e => e.EtkinlikDurumu == durum).ToList();
        }

        return View(etkinlikler); // Filtrelenmiş etkinlikleri view'a gönder
    }

    // Etkinlik Güncelleme Action Method
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
        //Burdaki errormodel yapısını global hale getirip sayfalarıda try catche alıp errorleri döndürebilirsin her sayfada basic olarak not olsun
        //Silmicem zaten bide sen dene bakim bişeyler var mı sey eklicem su konumlar aarası mesefa ölcemyi bizim harita şeysinde var o ordan çek istersen
        //Bişey lazım mı daha ? su anlık hayır :)) allah razı olsun iyi bi insana benzion kldsfjhklsdfg tamam bişi olursa yazarsın hadi ßß
        
        //if (!ModelState.IsValid)
        //{
        //    var errors = ModelState
        //        .Where(x => x.Value.Errors.Count > 0)
        //        .Select(x => new
        //        {
        //            PropertyName = x.Key,
        //            Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        //        })
        //        .ToArray();

        //    foreach (var item in errors)
        //    {
        //        foreach (var errorMessage in item.Errors)
        //        {
        //            ViewBag.m1 += "Error in " + item.PropertyName + ": " + errorMessage + Environment.NewLine;
        //        }
        //    }

        //    TempData["SuccessMessage"] = ViewBag.m1;
        //    return View(etkinlik);
        //}
        var mevcutEtkinlik = _context.Etkinlikler.FirstOrDefault(e => e.ID == etkinlik.ID);

        if (mevcutEtkinlik == null)
        {
            ViewBag.m1 = "Etkinlik Not Found!";
            TempData["SuccessMessage"] = ViewBag.m1;
            return View(etkinlik);//burda errorlü döndürmen lazım onuçözersin
        }

        // Etkinlik Durumu'nu güncelleme
        mevcutEtkinlik.EtkinlikAdi = etkinlik.EtkinlikAdi;
        mevcutEtkinlik.Aciklama = etkinlik.Aciklama;
        mevcutEtkinlik.Tarih = etkinlik.Tarih;
        mevcutEtkinlik.Saat = etkinlik.Saat;
        mevcutEtkinlik.EtkinlikSuresi = etkinlik.EtkinlikSuresi;
        mevcutEtkinlik.OlusturanKullaniciID = mevcutEtkinlik.OlusturanKullaniciID;
        mevcutEtkinlik.Konum = etkinlik.Konum;
        mevcutEtkinlik.Kategori = etkinlik.Kategori;
        // Etkinlik Durumu'nu güncellemiyoruz

        _context.Update(mevcutEtkinlik);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Etkinlik başarıyla güncellendi.";
        return RedirectToAction("EtkinlikListele");
    }




    // Etkinliği onayla
    [HttpPost]
    public IActionResult EtkinlikOnayla(int id)
    {
        var etkinlik = _context.Etkinlikler.FirstOrDefault(e => e.ID == id);

        if (etkinlik != null)
        {
            etkinlik.EtkinlikDurumu = "Onaylı"; // Etkinlik durumunu onaylı yap
            _context.SaveChanges(); // Değişiklikleri kaydet
        }

        return RedirectToAction("EtkinlikListele", new { durum = "Onaylı" }); // Onaylı etkinlikleri göster
    }

    // Etkinliği reddet
    [HttpPost]
    public IActionResult EtkinlikRed(int id)
    {
        var etkinlik = _context.Etkinlikler.FirstOrDefault(e => e.ID == id);

        if (etkinlik != null)
        {
            etkinlik.EtkinlikDurumu = "Reddedildi"; // Etkinlik durumunu reddedildi yap
            _context.SaveChanges(); // Değişiklikleri kaydet
        }

        return RedirectToAction("EtkinlikListele", new { durum = "Reddedildi" }); // Reddedilen etkinlikleri göster
    }


    // Etkinliği sil
    [HttpPost]
    public IActionResult EtkinlikSil(int id)
    {
        var etkinlik = _context.Etkinlikler.FirstOrDefault(e => e.ID == id);

        if (etkinlik != null)
        {
            // Etkinlik ile ilişkili katılımcıları bulup sil
            var katilimcilar = _context.Katilimcilar.Where(k => k.EtkinlikID == id).ToList();
            if (katilimcilar.Any())
            {
                _context.Katilimcilar.RemoveRange(katilimcilar); // Katılımcıları sil
            }

            // Etkinliği sil
            _context.Etkinlikler.Remove(etkinlik);
            _context.SaveChanges(); // Değişiklikleri kaydet
        }

        return RedirectToAction("EtkinlikListele"); // Etkinlikler listesine yönlendir
    }

    public IActionResult KatilimciListele()
    {
        var katilimcilar = _context.Katilimcilar
                                    .Include(k => k.Kullanici)     // Kullanıcı bilgilerini dahil et
                                    .Include(k => k.Etkinlik)      // Etkinlik bilgilerini dahil et
                                    .ToList();                     // Katılımcıları al

        return View(katilimcilar);  // Katılımcıları View'a gönder
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
