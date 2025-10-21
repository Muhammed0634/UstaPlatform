using UstaPlatform.Application;
using UstaPlatform.Domain;
using UstaPlatform.Infrastructure;
using System.Text;
using System.Globalization; // EKLENDİ (TL Sorunu ve formatlama için)

// --- PROJE DEMOSU (KOŞULLU SADAKAT, YÜK DENGELEME, ÇOKLU TALEP) ---
Console.OutputEncoding = Encoding.UTF8; // EKLENDİ (Türkçe karakterler için)
Console.WriteLine("UstaPlatform Simulasyonu Başlatılıyor...");
Console.Title = "UstaPlatform v2.0 (Gelişmiş Demo)"; // Güncellendi

// --- EKLENTİ KLASÖRÜ ---
string eklentiKlasoru = @"C:\UstaPlatformEklentiler";
Directory.CreateDirectory(eklentiKlasoru);
Console.WriteLine($"UYARI: Eklenti klasörü '{eklentiKlasoru}' olarak ayarlandı.");
Console.WriteLine("LÜTFEN TÜM KURAL DLL'LERİNİ (Haftasonu, Sadakat, HaftaIci) BU KLASÖRE KOPYAYIN.");
Console.WriteLine("\nHazır olduğunuzda devam etmek için ENTER'a basın...");
Console.ReadLine();

// --- VERİLERİ BAŞLATMA (GÜNCELLENDİ) ---
Console.WriteLine("\n--- Sistem Başlatılıyor: Test Verileri Oluşturuluyor ---");
IUstaRepository ustaRepo = new InMemoryUstaRepository();
IVatandasRepository vatandasRepo = new InMemoryVatandasRepository();

// TALEP 3 & 4: Yeni ustalar eklendi ve yük dengeleme için yükler ayarlandı
ustaRepo.Add(new Usta { AdSoyad = "Furkan Koç", UzmanlikAlani = "Tesisat", MevcutIsYuku = 0 });
ustaRepo.Add(new Usta { AdSoyad = "Mehmet Demir", UzmanlikAlani = "Elektrik", MevcutIsYuku = 0 }); // Yük dengeleme testi için 0 yapıldı
ustaRepo.Add(new Usta { AdSoyad = "Hasan Kaya", UzmanlikAlani = "tesisat", MevcutIsYuku = 1 }); // Yeni Tesisatçı
ustaRepo.Add(new Usta { AdSoyad = "Zeynep Kaya", UzmanlikAlani = "Elektrik", MevcutIsYuku = 0 });
ustaRepo.Add(new Usta { AdSoyad = "Murat Can", UzmanlikAlani = "Tesisat", MevcutIsYuku = 1 }); 

// TALEP 1 & 2: Yeni vatandaşlar ve mahalleler eklendi
var vatandasAli = new Vatandas { AdSoyad = "Ali kaplan", Adres = "Arcadia Merkez Mahallesi, 123. Sokak", Konum = (20, 30) };
var vatandasAyse = new Vatandas { AdSoyad = "Ayşe Yılmaz", Adres = "Arcadia Kenar Mahallesi, 456. Sokak", Konum = (70, 60) };
var vatandasFatma = new Vatandas { AdSoyad = "Fatma Solmaz", Adres = "Arcadia Merkez Mahallesi, 789. Bulvar", Konum = (10, 25) }; // Yeni Vatandaş (Merkez)
var vatandasDemir = new Vatandas { AdSoyad = " Efe Saymaz", Adres = "Çevre Yolu, 001. Cadde", Konum = (160, 180) }; // Yeni Vatandaş (Merkez Dışı)

vatandasRepo.Add(vatandasAli);
vatandasRepo.Add(vatandasAyse);
vatandasRepo.Add(vatandasFatma);
vatandasRepo.Add(vatandasDemir);

Console.WriteLine("Veriler yüklendi.");
Console.WriteLine("Tesisatçılar: Furkan (Yük: 0), Mehmet (Yük: 0), Hasan (Yük: 1)"); // Güncellendi
Console.WriteLine("Vatandaşlar: Ali (Merkez), Ayşe (Kenar), Fatma (Merkez), Efe (Çevre Yolu)"); // Güncellendi


// --- FİYAT MOTORUNU BAŞLAT ---
Console.WriteLine("\nFiyat Motoru (PricingEngine) Başlatılıyor...");
PricingEngine engine = new PricingEngine(eklentiKlasoru);

// --- PARA FORMATI (TL SORUNU DÜZELTİLDİ) ---
var culture = new CultureInfo("tr-TR");

// --- RASTGELE TARİH ÜRETİCİ ---
var random = new Random();
var haftaIciTarihi = new DateTime(2025, 10, 27, 14, 0, 0); // Pazartesi
var haftaSonuTarihi = new DateTime(2025, 10, 25, 14, 0, 0); // Cumartesi

// -----------------------------------------------------------------
// --- 1. TALEP: (Ali Veli - Merkez) - Sadakat UYGULANMALI ---
// -----------------------------------------------------------------
Console.WriteLine("\n--- 1. TALEP (Ali Kaplan - Merkez Mahallesi) ---");
var talep1 = new Talep { TalepEden = vatandasAli, Aciklama = "Mutfak musluğu sızdırıyor", GerekenUzmanlik = "Tesisat" };
var usta1 = ustaRepo.GetBestAvailable(talep1.GerekenUzmanlik);
Console.WriteLine($"En uygun usta bulundu: {usta1.AdSoyad} (İş Yükü: {usta1.MevcutIsYuku})");

usta1.MevcutIsYuku++;
Console.WriteLine($"'{usta1.AdSoyad}'  ustasına iş atandı.    YENİ İŞ YÜKÜ:   {usta1.MevcutIsYuku}");

DateTime secilenTarih1 = (random.Next(0, 2) == 0) ? haftaIciTarihi : haftaSonuTarihi;
var isEmri1 = new IsEmri { KaynakTalep = talep1, AtananUsta = usta1, Vatandas = vatandasAli, PlanlananTarih = secilenTarih1, Konum = vatandasAli.Konum }; // Konum eklendi
Console.WriteLine($"İş emri oluşturuldu. Planlanan tarih: {isEmri1.PlanlananTarih} ({isEmri1.PlanlananTarih.DayOfWeek})");
engine.CalculatePrice(isEmri1);

// GÜNCELLENDİ (Fiyat Detayları Gösteriliyor)
Console.WriteLine("----- Fiyat Dökümü -----");
foreach (var detay in isEmri1.FiyatDetaylari)
{
    Console.WriteLine($"> {detay}");
}
Console.WriteLine($"TOPLAM FİYAT: {isEmri1.Fiyat.ToString("C", culture)}");


// -----------------------------------------------------------------
// --- 2. TALEP: (Ayşe Yılmaz - Kenar) - Sadakat UYGULANMAMALI ---
// -----------------------------------------------------------------
Console.WriteLine("\n--- 2. TALEP (Ayşe Yılmaz - Kenar Mahalle) ---");
var talep2 = new Talep { TalepEden = vatandasAyse, Aciklama = "Banyo bataryası arızalı", GerekenUzmanlik = "Tesisat" };
var usta2 = ustaRepo.GetBestAvailable(talep2.GerekenUzmanlik); // Yük dengeleme (Mehmet'i seçmeli)
Console.WriteLine($"En uygun usta bulundu: {usta2.AdSoyad} (İş Yükü: {usta2.MevcutIsYuku})");

usta2.MevcutIsYuku++;
Console.WriteLine($"'{usta2.AdSoyad}' ustasına iş atandı. YENİ İŞ YÜKÜ: {usta2.MevcutIsYuku}");

DateTime secilenTarih2 = (random.Next(0, 2) == 0) ? haftaIciTarihi : haftaSonuTarihi;
var isEmri2 = new IsEmri { KaynakTalep = talep2, AtananUsta = usta2, Vatandas = vatandasAyse, PlanlananTarih = secilenTarih2, Konum = vatandasAyse.Konum }; // Konum eklendi
Console.WriteLine($"İş emri oluşturuldu. Planlanan tarih: {isEmri2.PlanlananTarih} ({isEmri2.PlanlananTarih.DayOfWeek})");
engine.CalculatePrice(isEmri2);

// GÜNCELLENDİ (Fiyat Detayları Gösteriliyor)
Console.WriteLine("--- Fiyat Dökümü ---");
foreach (var detay in isEmri2.FiyatDetaylari)
{
    Console.WriteLine($"> {detay}");
}
Console.WriteLine($"TOPLAM FİYAT: {isEmri2.Fiyat.ToString("C", culture)}");


// -----------------------------------------------------------------
// --- 3. TALEP (EKLENDİ): (Fatma Solmaz - Merkez) - Sadakat UYGULANMALI ---
// -----------------------------------------------------------------
Console.WriteLine("\n--- 3. TALEP (Fatma Solmaz - Merkez Mahallesi) ---");
var talep3 = new Talep { TalepEden = vatandasFatma, Aciklama = "Klozet sifonu çalışmıyor", GerekenUzmanlik = "Tesisat" };
var usta3 = ustaRepo.GetBestAvailable(talep3.GerekenUzmanlik); // Yük dengeleme (Hasan'ı seçmeli)
Console.WriteLine($"En uygun usta bulundu: {usta3.AdSoyad} (İş Yükü: {usta3.MevcutIsYuku})");

usta3.MevcutIsYuku++;
Console.WriteLine($"'{usta3.AdSoyad}' ustasına iş atandı. YENİ İŞ YÜKÜ: {usta3.MevcutIsYuku}");

DateTime secilenTarih3 = (random.Next(0, 2) == 0) ? haftaIciTarihi : haftaSonuTarihi;
var isEmri3 = new IsEmri { KaynakTalep = talep3, AtananUsta = usta3, Vatandas = vatandasFatma, PlanlananTarih = secilenTarih3, Konum = vatandasFatma.Konum }; // Konum eklendi
Console.WriteLine($"İş emri oluşturuldu. Planlanan tarih: {isEmri3.PlanlananTarih} ({isEmri3.PlanlananTarih.DayOfWeek})");
engine.CalculatePrice(isEmri3);

// GÜNCELLENDİ (Fiyat Detayları Gösteriliyor)
Console.WriteLine("--- Fiyat Dökümü ---");
foreach (var detay in isEmri3.FiyatDetaylari)
{
    Console.WriteLine($"> {detay}");
}
Console.WriteLine($"TOPLAM FİYAT: {isEmri3.Fiyat.ToString("C", culture)}");


// -----------------------------------------------------------------
// --- GÜNCEL İŞ YÜKLERİ (SADECE TESİSATÇILAR) ---
// -----------------------------------------------------------------
Console.WriteLine("\n--- GÜNCEL TESİSAT İŞ YÜKLERİ ---");
foreach (var usta in ustaRepo.GetAll().Where(u => u.UzmanlikAlani == "Tesisat"))
{
    Console.WriteLine($"- {usta.AdSoyad}: {usta.MevcutIsYuku}");
}

// -----------------------------------------------------------------
// --- ROTA TESTİ (EKLENDİ) ---
// -----------------------------------------------------------------
Console.WriteLine("\n--- Rota Optimizasyon Testi ---");

// 1. Yeni bir Rota nesnesi oluştur
var gunlukRota = new Rota();

// 2. Simülasyonda oluşturduğumuz İş Emirlerini Rota'ya ekle
// (isEmri1, isEmri2 ve isEmri3'ün konumlarını ekliyoruz)
gunlukRota.Add(isEmri1); // Add(IsEmri isEmri) metodunu kullanıyoruz
gunlukRota.Add(isEmri2);
gunlukRota.Add(isEmri3);

// 3. Rota'yı (IEnumerable olduğu için) foreach ile dön ve ekrana yazdır
// Burası GetEnumerator() metodunuzu otomatik olarak çağırır
Console.WriteLine("İşlerin ziyaret sırası (X'e göre sıralı):");
foreach (var durak in gunlukRota)
{
    Console.WriteLine($"- Konum: (X: {durak.X}, Y: {durak.Y})");
}

// 4. (EKLENDİ) Rota sınıfındaki yeni metodu çağır
double toplamMesafe = gunlukRota.HesaplaToplamMesafe();
Console.WriteLine($"Tahmini Toplam Mesafe: {toplamMesafe:F2} birim"); // F2 = virgülden sonra 2 basamak


Console.WriteLine("\n--- Simulasyon Tamamlandı ---");
Console.ReadLine();