using System.Reflection;
using UstaPlatform.Application;
using UstaPlatform.Domain;
using UstaPlatform.Rules.Haftasonu; // <-- Proje referansı eklendiğinde bu satır tanınmalıdır.
using Xunit;

namespace UstaPlatform.Tests
{
    // IDisposable: Test bittiğinde 'Dispose' metodu çalışır, böylece test klasörünü temizleyebiliriz.
    public class PricingEngineTests : IDisposable
    {
        private readonly string _testEklentiKlasoru;

        // Test başlamadan önce çalışan kurucu metot (Constructor)
        public PricingEngineTests()
        {
            // 1. Her test için benzersiz, geçici bir klasör oluştur
            _testEklentiKlasoru = Path.Combine(Path.GetTempPath(), $"UstaPlatformTest_{Guid.NewGuid()}");
            Directory.CreateDirectory(_testEklentiKlasoru);

            // 2. Gerekli DLL'leri bu geçici klasöre kopyala
            var kaynakKlasor = Path.GetDirectoryName(typeof(HaftasonuEkUcretiKurali).Assembly.Location)!;

            File.Copy( // Haftasonu kural DLL'si
                Path.Combine(kaynakKlasor, "UstaPlatform.Rules.Haftasonu.dll"),
                Path.Combine(_testEklentiKlasoru, "UstaPlatform.Rules.Haftasonu.dll")
            );
            File.Copy(
                Path.Combine(kaynakKlasor, "UstaPlatform.Domain.dll"),
                Path.Combine(_testEklentiKlasoru, "UstaPlatform.Domain.dll")
            );
            File.Copy(
                Path.Combine(kaynakKlasor, "UstaPlatform.Application.dll"),
                Path.Combine(_testEklentiKlasoru, "UstaPlatform.Application.dll")
            );
        }

        [Fact]
        public void CalculatePrice_HaftasonuKuraliniYukler_ve_FiyatiDogruHesaplar()
        {
            // --- ARRANGE ---
            // Motoru, geçici test klasörümüzle başlat
            var engine = new PricingEngine(_testEklentiKlasoru);
            var isEmri = new IsEmri
            {
                // Vatandaş ve Usta nesneleri oluşturulmalı, null olmamalı
                Vatandas = new Vatandas(), // Boş Vatandas nesnesi
                AtananUsta = new Usta(), // Boş Usta nesnesi
                PlanlananTarih = new DateTime(2025, 10, 25) // Cumartesi

            };
            var temelFiyat = 100.0m;
            var beklenenFiyat = temelFiyat * 1.20m; // %20 hafta sonu zammı

            // --- ACT ---
            var hesaplananFiyat = engine.CalculatePrice(isEmri); 

            // --- ASSERT ---
            Assert.Equal(beklenenFiyat, hesaplananFiyat); 
        }

        // Test bittikten sonra çalışan metot
        public void Dispose()
        {
            // Oluşturulan geçici klasörü ve içindekileri sil
            if (Directory.Exists(_testEklentiKlasoru))
            {
                Directory.Delete(_testEklentiKlasoru, true); 
            }
        }
    }
}