using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using yazlab2mvc.Models;

namespace yazlab2mvc.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly Context _context;

        public KullaniciController(Context context)
        {
            _context = context;
        }

       
        public IActionResult Profil()
        {
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == kullaniciID);

            if (kullanici != null)
            {
                ViewData["KullaniciAdi"] = kullanici.Ad + " " + kullanici.Soyad; 
            }

            return View(kullanici); 
        }

        
        public IActionResult Puanlar()
        {
           
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");
            if (!kullaniciID.HasValue)
            {
                TempData["Message"] = "Lütfen puan geçmişinizi görmek için giriş yapın.";
                return RedirectToAction("GirisYap", "Kullanici");
            }

            try
            {
               
                var puanlar = _context.Puanlar
                    .Where(p => p.KullaniciID == kullaniciID.Value)
                    .OrderByDescending(p => p.KazanilanTarih) 
                    .ToList();

                
                if (!puanlar.Any())
                {
                    ViewBag.Message = "Henüz puan kazanmadınız.";
                }

                return View(puanlar);
            }
            catch (Exception ex)
            {
               
                ViewBag.Message = "Puan geçmişinizi yüklerken bir hata oluştu: " + ex.Message;
                return View(new List<Puanlar>()); 
            }
        }

        public IActionResult Harita()
        {
            
            var etkinlikler = _context.Etkinlikler
                .Select(e => new { e.EtkinlikAdi, e.Konum }) 
                .ToList();

            return View(etkinlikler); 
        }


        [HttpGet]
        public IActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> KayitOl(Kullanicilar kullanici)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                   
                    kullanici.Sifre = Sifreleme.sifrele(kullanici.Sifre, "kkkk1234");

                   
                    var ilgiAlanlari = Request.Form["IlgiAlanlari"];
                    if (ilgiAlanlari.Count > 0)
                    {
                        
                        kullanici.IlgiAlanlari = string.Join(", ", ilgiAlanlari);
                    }
                    System.Diagnostics.Debug.WriteLine("IF"+ Request.Form.Files.Count);
                    Console.WriteLine("IF"+ Request.Form.Files.Count);
                    if (Request.Form.Files.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("TRY");
                        Console.WriteLine("TRY");
                        try
                        {
                            var file = Request.Form.Files[0];
                            string dosyaAdi = Path.GetFileName(file.FileName);
                            string uzanti = Path.GetExtension(file.FileName);
                            string yeniDosyaAdi = Guid.NewGuid().ToString() + uzanti; 
                            string yol = Path.Combine("wwwroot/Image", yeniDosyaAdi); 

                            using (var stream = new FileStream(yol, FileMode.Create))
                            {
                                await file.CopyToAsync(stream); 
                            }

                            kullanici.ProfilFotografi = "/Image/" + yeniDosyaAdi; 
                            System.Diagnostics.Debug.WriteLine("Hata Yok:" + yol);
                            Console.WriteLine("Hata Yok:" + yol);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Hata:" + ex.Message);
                            Console.WriteLine("Hata:" + ex.Message);
                        }
                        
                    }
                    System.Diagnostics.Debug.WriteLine("ENDIF"+ Request.Form.Files.Count);
                    Console.WriteLine("ENDIF"+ Request.Form.Files.Count);
                    _context.Kullanicilar.Add(kullanici); 
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Kayıt başarılı!";
                    return RedirectToAction("GirisYap");
                }
                catch (Exception)
                {
                    TempData["Message"] = "Bir hata oluştu!";
                    return RedirectToAction("KayitOl");
                }
            }
            return View(kullanici);
        }
        
        [HttpGet]
        public IActionResult GirisYap()
        {
            return View();
        }

        public IActionResult KullaniciArayuz()
        {
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");

            if (kullaniciID.HasValue)
            {
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == kullaniciID.Value);

                if (kullanici != null)
                {
                    
                    ViewData["KullaniciAdi"] = kullanici.Ad + " " + kullanici.Soyad;
                }
            }

            return View(); 
        }
        
        [HttpPost]
        public IActionResult GirisYap(string kullaniciAdi, string sifre)
        {
            sifre = Sifreleme.sifrele(sifre, "kkkk1234");
            var kullanici = _context.Kullanicilar

                .FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi && k.Sifre == sifre);

            if (kullanici != null)
            {
                // Kullanıcı ID'sini oturuma ekle
                HttpContext.Session.SetInt32("KullaniciID", kullanici.ID);

                // Başarılı girişten sonra yönlendir
                return RedirectToAction("KullaniciArayuz", "Kullanici");
            }
            else
            {
                ViewBag.Message = "Kullanıcı adı veya şifre yanlış. Lütfen tekrar deneyin.";
                ViewBag.IsSuccess = false;
                return View();
            }
        }

        [HttpGet]
        public IActionResult EtkinlikOlustur()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EtkinlikOlustur(Etkinlikler etkinlik)
        {
            try
            {
               
                var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");
                if (!kullaniciID.HasValue)
                {
                    ViewBag.Message = "Kullanıcı giriş yapmamış. Lütfen giriş yapın.";
                    return RedirectToAction("GirisYap", "Kullanici");
                }

               
                etkinlik.OlusturanKullaniciID = kullaniciID.Value;

               
                etkinlik.EtkinlikDurumu = "Onay Bekliyor";

                
                _context.Etkinlikler.Add(etkinlik);
                await _context.SaveChangesAsync(); 

               
                var katilimci = new Katilimcilar
                {
                    KullaniciID = kullaniciID.Value,
                    EtkinlikID = etkinlik.ID
                };
                _context.Katilimcilar.Add(katilimci);
                await _context.SaveChangesAsync();
                
                await PuanEkle(kullaniciID.Value, 15, "Etkinlik oluşturma");
                ViewBag.Message = "Etkinlik başarıyla oluşturuldu ve 15 puan eklendi katılım eklendi!";
                return RedirectToAction("KullaniciArayuz", "Kullanici");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Bir hata oluştu: " + ex.Message;
                return View(etkinlik);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EtkinligeKatil(int etkinlikID)
        {
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");

            if (!kullaniciID.HasValue)
            {
                TempData["Message"] = "Kullanıcı giriş yapmamış. Lütfen giriş yapın.";
                return RedirectToAction("EtkinlikleriGoruntule");
            }

            var mevcutKatilim = _context.Katilimcilar
                .FirstOrDefault(k => k.KullaniciID == kullaniciID.Value && k.EtkinlikID == etkinlikID);

            if (mevcutKatilim != null)
            {
                TempData["Message"] = "Bu etkinliğe zaten katıldınız.";
                return RedirectToAction("EtkinlikleriGoruntule");
            }

            var etkinlik = await _context.Etkinlikler.FindAsync(etkinlikID);
            if (etkinlik == null)
            {
                TempData["Message"] = "Etkinlik bulunamadı.";
                return RedirectToAction("EtkinlikleriGoruntule");
            }

            var yeniEtkinlikBaslangic = etkinlik.Tarih.Date + etkinlik.Saat;
            var yeniEtkinlikBitis = yeniEtkinlikBaslangic + etkinlik.EtkinlikSuresi;

            var katildigiEtkinlikler = _context.Katilimcilar
                .Where(k => k.KullaniciID == kullaniciID.Value)
                .Select(k => new
                {
                    Baslangic = k.Etkinlik.Tarih.Date + k.Etkinlik.Saat,
                    Bitis = k.Etkinlik.Tarih.Date + k.Etkinlik.Saat + k.Etkinlik.EtkinlikSuresi
                })
                .ToList();

            bool cakismaVar = katildigiEtkinlikler.Any(e =>
                yeniEtkinlikBaslangic < e.Bitis && yeniEtkinlikBitis > e.Baslangic);

            if (cakismaVar)
            {
                TempData["Message"] = "Bu etkinliğe katılamazsınız, çünkü başka bir etkinlik ile çakışıyor.";
                return RedirectToAction("EtkinlikleriGoruntule");
            }

            var yeniKatilim = new Katilimcilar
            {
                KullaniciID = kullaniciID.Value,
                EtkinlikID = etkinlikID
            };

            _context.Katilimcilar.Add(yeniKatilim);
            await _context.SaveChangesAsync();

            
            var dahaOnceKatildiMi = _context.Katilimcilar
                .Any(k => k.KullaniciID == kullaniciID.Value && k.EtkinlikID != etkinlikID);

           
            if (!dahaOnceKatildiMi)
            {
                await PuanEkle(kullaniciID.Value, 20, "İlk etkinlik katılımı");
                TempData["Message"] = "Etkinliğe başarıyla katıldınız, 20 puan kazandınız!";
            }
            else
            {
                await PuanEkle(kullaniciID.Value, 10, "Etkinlik katılımı");
                TempData["Message"] = "Etkinliğe başarıyla katıldınız, 10 puan kazandınız!";
            }

            return RedirectToAction("EtkinlikleriGoruntule");
        }


        public IActionResult OnayBekleyenEtkinlikler()
        {
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");

            if (!kullaniciID.HasValue)
            {
                
                return RedirectToAction("GirisYap", "Kullanici");
            }

            
            var etkinlikler = _context.Etkinlikler
                                      .Where(e => e.OlusturanKullaniciID == kullaniciID.Value &&
                                                  e.EtkinlikDurumu == "Onay Bekliyor")
                                      .ToList();

            return View(etkinlikler); 
        }
    

        public IActionResult ReddedilenEtkinlikler()
        {
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");

            if (!kullaniciID.HasValue)
            {
                
                return RedirectToAction("GirisYap", "Kullanici");
            }

           
            var etkinlikler = _context.Etkinlikler
                                      .Where(e => e.OlusturanKullaniciID == kullaniciID.Value &&
                                                  e.EtkinlikDurumu == "Reddedildi")
                                      .ToList();

            return View(etkinlikler); 
        }

        public IActionResult EtkinlikleriGoruntule()
        {
           
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");

           
            if (kullaniciID == null)
            {
                
                return RedirectToAction("GirisYap", "Kullanici");
            }

           
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == kullaniciID);
            if (kullanici == null || string.IsNullOrEmpty(kullanici.Konum))
            {
                
                TempData["Hata"] = "Kullanıcı konumu bulunamadı. Lütfen profilinizi güncelleyiniz.";
                return RedirectToAction("ProfilGuncelle", "Kullanici");
            }

           
            ViewBag.KullaniciKonum = kullanici.Konum;

            
            var etkinlikler = _context.Etkinlikler
                                      .Include(e => e.Katilimcilar) 
                                      .Where(e => e.EtkinlikDurumu == "Onaylı" && 
                                                   !e.Katilimcilar.Any(k => k.KullaniciID == kullaniciID.Value)) 
                                      .ToList();

           
            return View(etkinlikler);
        }

        public IActionResult KatildigimEtkinlikler()
        {
            
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");

           
            if (!kullaniciID.HasValue)
            {
                return RedirectToAction("Giris", "Admin"); 
            }

            
            var katildigiEtkinlikler = _context.Katilimcilar
                                                .Include(k => k.Etkinlik) 
                                                .Where(k => k.KullaniciID == kullaniciID.Value &&
                                                            k.Etkinlik.EtkinlikDurumu == "Onaylı") 
                                                .Select(k => k.Etkinlik) 
                                                .ToList();

            return View(katildigiEtkinlikler); 
        }




        [HttpGet]
        public IActionResult MesajGonder(int etkinlikID)
        {
            
            var etkinlik = _context.Etkinlikler.FirstOrDefault(e => e.ID == etkinlikID);
            if (etkinlik == null)
            {
                TempData["ErrorMessage"] = "Etkinlik bulunamadı.";
                return RedirectToAction("KatildigimEtkinlikler", "Kullanici");
            }

            
            var mesajlar = _context.Mesajlar
                .Where(m => m.EtkinlikID == etkinlikID)
                .OrderByDescending(m => m.GonderimZamani)
                .ToList();

         
            var gondericiIDs = mesajlar.Select(m => m.GondericiID).Distinct().ToList();

           
            var kullaniciAdiListesi = _context.Kullanicilar
                .Where(k => gondericiIDs.Contains(k.ID))
                .ToDictionary(k => k.ID, k => k.KullaniciAdi);  

           
            var mesajlarWithKullaniciAdi = mesajlar.Select(m => new MesajViewModel
            {
                MesajMetni = m.MesajMetni,
                GonderimZamani = m.GonderimZamani,
                KullaniciAdi = kullaniciAdiListesi.ContainsKey(m.GondericiID) ? kullaniciAdiListesi[m.GondericiID] : "Bilinmiyor"
            }).ToList();

            
            var model = new MesajGonderViewModel
            {
                Mesajlar = mesajlarWithKullaniciAdi,
                EtkinlikAdi = etkinlik.EtkinlikAdi,
                EtkinlikID = etkinlikID
            };

            return View(model);
        }



        [HttpPost]
        public IActionResult MesajGonder(int etkinlikID, string mesaj)
        {
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");

            if (!kullaniciID.HasValue)
            {
                ViewBag.Message = "Kullanıcı giriş yapmamış. Lütfen giriş yapın.";
                return RedirectToAction("GirisYap", "Kullanici");
            }

           
            var yeniMesaj = new Mesajlar
            {
                GondericiID = kullaniciID.Value,
                AliciID = kullaniciID.Value, 
                EtkinlikID = etkinlikID,
                MesajMetni = mesaj,
                GonderimZamani = DateTime.Now
            };

            _context.Mesajlar.Add(yeniMesaj);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Mesaj başarıyla gönderildi!";
            return RedirectToAction("MesajGonder", new { etkinlikID = etkinlikID });
        }

        
        [HttpGet]
        public IActionResult SifreYenileme()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> SifreYenileme(string kullaniciAdi, string eposta, string telefonNumarasi, string yeniSifre)
        {
            // Formdaki alanların boş olup olmadığını kontrol ediyoruz
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(eposta) || string.IsNullOrEmpty(telefonNumarasi) || string.IsNullOrEmpty(yeniSifre))
            {
                ViewBag.Message = "Tüm alanları doldurduğunuzdan emin olun.";
                ViewBag.IsSuccess = false;
                return View();
            }

            // Kullanıcı adı, E-posta ve Telefon Numarası ile kullanıcıyı veritabanında arıyoruz
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(u => u.KullaniciAdi == kullaniciAdi && u.Eposta == eposta && u.TelefonNumarasi == telefonNumarasi);

            // Kullanıcı bulunamazsa hata mesajı gösteriyoruz
            if (kullanici == null)
            {
                ViewBag.Message = "Bu bilgilerle kayıtlı kullanıcı bulunamadı.";
                ViewBag.IsSuccess = false;
                return View();
            }
      
             yeniSifre = Sifreleme.sifrele(yeniSifre,"kkkk1234");
            kullanici.Sifre = yeniSifre;

           
            // Yeni şifreyi güncelliyoruz
            kullanici.Sifre = yeniSifre;

            try
            {
                _context.Update(kullanici);
                await _context.SaveChangesAsync();

                ViewBag.Message = "Şifreniz başarıyla güncellenmiştir.";
                ViewBag.IsSuccess = true;

                // Kullanıcıyı giriş sayfasına yönlendiriyoruz
                return RedirectToAction("GirisYap", "Kullanici");
            }
            catch (Exception ex)
            {
                // Hata durumu
                ViewBag.Message = "Şifre güncellenirken bir hata oluştu: " + ex.Message;
                ViewBag.IsSuccess = false;
                return View();
            }
        }


        [HttpGet]
        public IActionResult ProfilGuncelle()
        {
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");
            if (kullaniciID == null)
            {
                return RedirectToAction("GirisYap", "Kullanici");
            }

            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == kullaniciID);
            if (kullanici == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return View(kullanici);
        }

        [HttpPost]
        public async Task<IActionResult> ProfilGuncelle(Kullanicilar guncelKullanici)
        {
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");
            if (kullaniciID == null)
            {
                return RedirectToAction("GirisYap", "Kullanici");
            }

            var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == kullaniciID);
            if (mevcutKullanici == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            
            mevcutKullanici.Ad = guncelKullanici.Ad;
            mevcutKullanici.Soyad = guncelKullanici.Soyad;
            mevcutKullanici.Eposta = guncelKullanici.Eposta;
            mevcutKullanici.TelefonNumarasi = guncelKullanici.TelefonNumarasi;
            mevcutKullanici.Konum = guncelKullanici.Konum;

            
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var dosya = Request.Form.Files[0];
                    if (dosya != null && dosya.Length > 0)
                    {
                        string uzanti = Path.GetExtension(dosya.FileName);
                        string yeniDosyaAdi = Guid.NewGuid().ToString() + uzanti; 
                        string kaydetmeYolu = Path.Combine("wwwroot/Image", yeniDosyaAdi);

                       
                        using (var stream = new FileStream(kaydetmeYolu, FileMode.Create))
                        {
                            await dosya.CopyToAsync(stream);
                        }

                       
                        if (!string.IsNullOrEmpty(mevcutKullanici.ProfilFotografi))
                        {
                            string eskiDosyaYolu = Path.Combine("wwwroot", mevcutKullanici.ProfilFotografi.TrimStart('/'));
                            if (System.IO.File.Exists(eskiDosyaYolu))
                            {
                                System.IO.File.Delete(eskiDosyaYolu);
                            }
                        }

                       
                        mevcutKullanici.ProfilFotografi = "/Image/" + yeniDosyaAdi;
                    }
                }

                await _context.SaveChangesAsync();
                ViewBag.Message = "Bilgileriniz başarıyla güncellendi.";
                ViewBag.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Güncelleme sırasında bir hata oluştu: " + ex.Message;
                ViewBag.IsSuccess = false;
            }

            return View(mevcutKullanici);
        }
        public async Task PuanEkle(int kullaniciID, int puan, string aciklama)
        {
            var yeniPuan = new Puanlar
            {
                KullaniciID = kullaniciID,
                KazanilanTarih = DateTime.Now,
                Puan = puan
            };

            _context.Puanlar.Add(yeniPuan);
            await _context.SaveChangesAsync();

        }
        public IActionResult GecmisEtkinliklereGoreOnerme()
        {
           
            var kullaniciID = HttpContext.Session.GetInt32("KullaniciID");

            if (!kullaniciID.HasValue)
            {
               
                return RedirectToAction("Login", "Account");
            }

           
            var katildigiEtkinlikler = _context.Katilimcilar
                .Include(k => k.Etkinlik)
                .Where(k => k.KullaniciID == kullaniciID.Value && k.Etkinlik.EtkinlikDurumu == "Onaylı") 
                .Select(k => k.Etkinlik) 
                .ToList();

           
            var katildiginizKategoriler = katildigiEtkinlikler
                .Select(e => e.Kategori) 
                .Distinct() 
                .ToList();

            if (katildiginizKategoriler.Count == 0)
            {
                TempData["Hata"] = "Katıldığınız etkinlikler bulunamadı.";
                return RedirectToAction("Index", "Home");
            }

           
            var onerilenEtkinlikler = _context.Etkinlikler
                .Where(e => katildiginizKategoriler.Contains(e.Kategori) && e.EtkinlikDurumu == "Onaylı") 
                .Where(e => !katildigiEtkinlikler.Contains(e)) 
                .ToList();

           
            if (onerilenEtkinlikler.Count == 0)
            {
                TempData["Hata"] = "Yeni önerilen etkinlik bulunamadı.";
            }

            return View(onerilenEtkinlikler); 
        }

       
        public IActionResult OnerilenEtkinlikler()
        {
           
            var kullaniciIlgiAlanlari = _context.Kullanicilar
                .Where(k => k.ID == HttpContext.Session.GetInt32("KullaniciID"))
                .Select(k => k.IlgiAlanlari)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(kullaniciIlgiAlanlari))
            {
                TempData["Hata"] = "İlgi alanlarınız boş. Lütfen profilinizi güncelleyiniz.";
                return RedirectToAction("ProfilGuncelle", "Kullanici");
            }

           
            var ilgiAlanlariListesi = kullaniciIlgiAlanlari.Split(',').Select(i => i.Trim()).ToList();

           
            var etkinlikler = _context.Etkinlikler
                .Where(e => ilgiAlanlariListesi.Contains(e.Kategori))
                .ToList();

            return View(etkinlikler);
        }
    }
}