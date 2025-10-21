using UstaPlatform.Application; // IPricingRule'u almak için
using UstaPlatform.Domain;

namespace UstaPlatform.Rules.Haftasonu
{
    // Bakın, bu sınıf IPricingRule arayüzünü uyguluyor.
    // PricingEngine bunu DLL içinde bulacak.
    public class HaftasonuEkUcretiKurali : IPricingRule
    {
        public string KuralAdi => "Hafta Sonu Ek Ücreti";

        public decimal Hesapla(IsEmri isEmri, decimal mevcutFiyat)
        {
            var gun = isEmri.PlanlananTarih.DayOfWeek;

            if (gun == DayOfWeek.Saturday || gun == DayOfWeek.Sunday)
            {
                return mevcutFiyat + 20.0m; // Hafta sonu ek ücreti 20.0 TL
            }

            return mevcutFiyat; // Değişiklik yok
        }
    }
}