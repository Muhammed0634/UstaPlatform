using UstaPlatform.Application;
using UstaPlatform.Domain;

namespace UstaPlatform.Rules.Sadakat
{
    // Bu da IPricingRule'u uyguluyor.
    public class SadakatIndirimiKurali : IPricingRule
    {
        public string KuralAdi => "Sadakat İndirimi";

        public decimal Hesapla(IsEmri isEmri, decimal mevcutFiyat)
        {
            // Normalde vatandaşın geçmiş işlerine bakılır.
            // Biz demo için basitçe adresinde "Merkez" yazıyorsa indirim yapalım.
            if (isEmri.Vatandas.Adres.Contains("Merkez"))
            {
                
              return mevcutFiyat * 0.40m; 
            }
            return mevcutFiyat;
        }
    }
}