# Etkinlik Yönetim Sistemi  

## Kocaeli Üniversitesi  
### Bilgisayar Mühendisliği Bölümü  
### Yazılım Lab. I - 2. Proje  


---  

## Proje Hakkında  

Etkinlik Yönetim Sistemi, kullanıcıların etkinlikleri görüntüleyebileceği, katılabileceği ve kendi etkinliklerini oluşturabileceği bir platformdur. Ayrıca kullanıcılar arasında sohbet imkanı ve etkinlik haritasına erişim sağlanmaktadır.  

## Proje Mimarisi  

### 1. Frontend (Kullanıcı Arayüzü)  
Kullanıcıların etkinlikleri görüntüleyebileceği ve katılabileceği bir arayüz tasarlanacaktır.  

#### Ana Bileşenler:  
- **Ana Sayfa:** Kullanıcıların önerilen etkinlikleri görebileceği ve kendi etkinliklerini ekleyebileceği sayfa.  
- **Etkinlik Sayfası:** Seçilen etkinliğin detaylarını görüntüleyebileceği sayfa.  
- **Sohbet:** Kullanıcılar arasında mesajlaşma imkanı sağlamaktadır.  
- **Kullanıcı Profili:** Kullanıcıların katıldıkları etkinlikleri görüntüleyebileceği profil sayfası.  
- **Admin Profili:** Platformun yönetimi için admin arayüzü.  
- **Giriş Ekranı:** Kullanıcıların platforma giriş yapabileceği veya yeni hesap oluşturabileceği alan.  

### 2. Backend (Sunucu Tarafı)  
Sunucu tarafında kullanıcı ve etkinlik verilerinin yönetimi yapılacak.  

#### a. Kullanıcı Yönetimi:  
- Giriş, kayıt, şifre sıfırlama ve kullanıcı doğrulama işlemleri.  
- Kullanıcı profili oluşturma ve güncelleme ile ilgi alanları ekleme.  
- Kullanıcı rolleri (kullanıcı, admin) tanımlanarak yetkilendirme.  
- Kullanıcıların kayıt olma süreci ve bilgi güncelleme işlemleri.  

#### b. Etkinlik Yönetimi:  
- Kullanıcıların etkinlik oluşturma, güncelleme ve silme işlemleri.  
- Etkinlik detayları (isim, tarih, saat, açıklama, konum, kategori).  
- Admin, etkinlikleri yöneterek onaylama, silme veya düzenleme yapacaktır.  

#### c. Etkinlik Öneri Sistemi:  
- Kullanıcı ilgi alanlarına ve katılım geçmişine göre öneriler sunan kural tabanlı sistem.  

#### ç. Çakışma Algoritması:  
- Kullanıcıların katılmak istedikleri etkinliklerin tarih ve zaman açısından çakışıp çakışmadığını kontrol eder.  

#### d. Admin Paneli:  
- Tüm sistemin yönetimi için admin arayüzü ile kullanıcı ve etkinlik yönetimi.  

#### e. Mesajlaşma Paneli:  
- Her etkinlik için ayrı sohbet alanı, katılımcıların mesajlaşabilme imkanı.  

## Veritabanı Tasarımı  

Projede ilişkisel bir veritabanı kullanılacaktır. Aşağıdaki tablolar, etkinlikler, kullanıcılar, katılımcılar ve mesajlar gibi verileri yönetmek için tasarlanmıştır.  

### Veritabanı Tabloları:  
- **Kullanıcılar:**  
  - ID  
  - Kullanıcı adı  
  - Şifre  
  - E-posta  
  - Konum  
  - İlgi alanları  
  - Ad  
  - Soyad  
  - Doğum tarihi  
  - Cinsiyet  
  - Telefon numarası  
  - Profil fotoğrafı  

- **Etkinlikler:**  
  - ID  
  - Etkinlik adı  
  - Açıklama  
  - Tarih  
  - Saat  
  - Etkinlik süresi  
  - Konum  
  - Kategori  

- **Katılımcılar:**  
  - Kullanıcı ID  
  - Etkinlik ID  

- **Mesajlar:**  
  - Mesaj ID  
  - Gönderici ID  
  - Alıcı ID  
  - Mesaj Metni  
  - Gönderim Zamanı  

- **Puanlar:**  
  - Kullanıcı ID  
  - Puanlar  
  - Kazanılan Tarih  

---  
## Ana Fonksiyonlar ve Özellikler  

### 1. Kişiselleştirilmiş Etkinlik Öneri Sistemi (Kural Tabanlı)  
Kullanıcıların ilgi alanlarına, etkinlik geçmişlerine ve bulundukları konuma göre kişiselleştirilmiş etkinlik önerileri yapılacaktır.   

#### Öneri Kuralları:  
- **İlgi Alanı Uyum Kuralı:** Kullanıcının belirttiği ilgi alanlarına uygun etkinlikler öncelikli olarak önerilecektir.  
- **Katılım Geçmişi Kuralı:** Kullanıcının geçmişte katıldığı etkinliklerin türüne göre benzer etkinlikler önerilecektir.  
- **Coğrafi Konum Kuralı:** Kullanıcının bulunduğu coğrafi bölgeye uygun etkinlikler önerilecektir.  

### 2. Harita ve Rota Planlama  
Kullanıcılar, etkinliklerin konumlarını harita üzerinden görebilecek ve en uygun rota önerilerini alabilecektir.  

#### 1. Konum Bazlı Etkinlikler:  
- Etkinliklerin konumları harita üzerinde işaretlenecek, detay sayfalarında görsel olarak sunulacaktır.  

#### 2. Rota Planlama:  
- Kullanıcılar, başlangıç noktasından etkinliğe ulaşmaları için en uygun rota önerileri alacaklar. API kullanılarak gerçek zamanlı rota hesaplanacaktır.  

### 3. Oyunlaştırma Sistemi  
Kullanıcılar etkinliklere katıldıkça puan kazanacaklar.  

#### Ana Bileşenler:  
1. **Puanlama Sistemi:**  
   - Etkinliğe Katılım: 10 puan  
   - Etkinlik Oluşturma: 15 puan  
   - İlk Katılım: 20 puan  

2. **Puanlama Matematiği:**  
   - Örneğin, toplam puan hesaplama:  
     - Katılım Puanı: 5 etkinlik × 10 puan = 50 puan  
     - Oluşturma Puanı: 2 etkinlik × 15 puan = 30 puan  
     - Toplam Puan: 50 + 30 + 20 = 100 puan  

### 4. Tarih ve Zaman Çakışma Algoritması  
Kullanıcılar, etkinliklere katılırken zaman çakışmalarını önlemek için bir sistem kullanacaktır.  

#### Ana Bileşenler:  
1. **Zaman Çakışması Algoritması:**  
   - Kullanıcının katılmak istediği etkinlik ile geçmişte katıldığı etkinliklerin zaman dilimleri karşılaştırılacaktır.  
   - Çakışma varsa, kullanıcıya bildirim gönderilecektir.  

### 5. Mesajlaşma Sistemi  
Kullanıcıların etkinlikler hakkında bilgi alışverişi ve etkileşimde bulunmaları için bir mesajlaşma sistemi geliştirilmiştir.  

#### Ana Bileşenler:  
- Kullanıcıların etkinlik sayfasında mesajlaşmaları ve bildirimler alması sağlanacaktır.  
- Mesaj geçmişi, kullanıcıların önceki tartışmalara erişimini kolaylaştıracaktır.  

---  

## Proje Gereksinimleri  
1. Kullanıcı kayıt, giriş, şifre sıfırlama ve profil güncelleme işlemleri.  
2. Etkinlik oluşturma, güncelleme, silme ve katılım sağlama işlevleri.  
3. Kural tabanlı kişiselleştirilmiş öneri sistemi.  
4. Etkinlikler arasında zaman çakışmalarını tespit etme sistemi.  
5. Mesajlaşma işlevselliği.  
6. Etkinliklerin harita üzerinde gösterilmesi ve en uygun rotasyonu bulmak
## Proje Hedefleri  
- Kullanıcı dostu arayüz ile etkinlik yönetimini kolaylaştırmak.  
- Veritabanı ve sunucu tarafı yönetimini etkili bir şekilde sağlamak.  
- Kullanıcı deneyimini artırmak
