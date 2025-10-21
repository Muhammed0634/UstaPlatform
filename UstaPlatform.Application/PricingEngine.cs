using System.Reflection; // DLL'leri okumak için
using UstaPlatform.Domain;

namespace UstaPlatform.Application
{
    // GEREKLİLİK 3.A.1 (OCP) ve 4 (Plug-in Mimarisi)
    // Bu sınıf, yeni kurallar eklendiğinde DEĞİŞMEZ (Kapalı).
    // Ama yeni kuralları (DLL) dışarıdan alabilir (Açık).
    public class PricingEngine
    {
        private readonly List<IPricingRule> _kurallar = new();

        // Motor, kuralların nerede olduğunu bilmek zorundadır.
        public PricingEngine(string eklentiKlasoruYolu)
        {
            Console.WriteLine($"[PricingEngine] Eklenti klasörü taranıyor: {eklentiKlasoruYolu}");
            LoadRules(eklentiKlasoruYolu);
        }

        private void LoadRules(string eklentiKlasoru)
        {
            if (!Directory.Exists(eklentiKlasoru))
            {
                Console.WriteLine($"[PricingEngine] UYARI: Eklenti klasörü bulunamadı.");
                return;
            }

            // Klasördeki tüm .dll dosyalarını bul
            var dllDosyalari = Directory.GetFiles(eklentiKlasoru, "*.dll");

            foreach (var dosya in dllDosyalari)
            {
                try
                {
                    // 1. DLL'i hafızaya yükle
                    var assembly = Assembly.LoadFrom(dosya);

                    // 2. DLL içindeki tüm sınıfları (tipleri) tara
                    var tipler = assembly.GetTypes()
                        .Where(t => typeof(IPricingRule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);   // IPricingRule'u uygulayan somut sınıflar

                    // 3. IPricingRule'u uygulayan her sınıfı bul ve oluştur (instance)
                    foreach (var tip in tipler)
                    {
                        var kural = (IPricingRule)Activator.CreateInstance(tip)!; // Instance oluştur   
                        _kurallar.Add(kural);
                        Console.WriteLine($"[PricingEngine] BAŞARILI: Kural yüklendi -> {kural.KuralAdi} ({Path.GetFileName(dosya)})"); // Başarı mesajı
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PricingEngine] HATA: Kural yüklenemedi: {Path.GetFileName(dosya)}. Detay: {ex.Message}"); // Hata ayıklama için detaylı mesaj
                }
            }
            Console.WriteLine($"[PricingEngine] Toplam {_kurallar.Count} kural yüklendi.");
        }

        // Fiyatı hesapla
        public decimal CalculatePrice(IsEmri isEmri)
        {
            // Fiyatlandırma bir "kompozisyondur".
            // Her kural, bir öncekinin sonucunu alıp kendi etkisini ekler.
            decimal fiyat = 100.0m; // Temel hizmet bedeli
            Console.WriteLine($"  -> Temel Hizmet Bedeli: {fiyat:C}");

            foreach (var kural in _kurallar.OrderBy(k => k.KuralAdi)) // (Sıralı uygula)
            {
                decimal oncekiFiyat = fiyat;
                fiyat = kural.Hesapla(isEmri, fiyat);
                decimal etki = fiyat - oncekiFiyat;

                if (etki != 0)
                {
                    Console.WriteLine($"  -> Kural Uygulandı ({kural.KuralAdi}): {etki:C} (Yeni Toplam: {fiyat:C})");
                }
            }

            isEmri.Fiyat = fiyat;
            Console.WriteLine($"  => Nihai Fiyat: {fiyat:C}");
            return fiyat;
        }
    }
}