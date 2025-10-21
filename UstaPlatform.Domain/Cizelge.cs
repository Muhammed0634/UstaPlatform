using System.Collections;

namespace UstaPlatform.Domain
{
    // Ustaların iş emri takvimi
    public class Cizelge
    {
        // DateOnly: Sadece tarih bilgisi tutar (saat olmadan), .NET 6+
        private readonly Dictionary<DateOnly, List<IsEmri>> _gunlukIsler = new(); // Her gün için iş emri listesi

        // GEREKLİLİK 3.B.3: Dizinleyici (Indexer)
        // Bu sayede Cizelge[gun] yazarak o günün işlerine erişebiliriz.
        public List<IsEmri> this[DateOnly gun] 
        {
            get
            {
                // Eğer o gün için bir liste yoksa, boş bir liste oluştur
                _gunlukIsler.TryAdd(gun, new List<IsEmri>()); 
                return _gunlukIsler[gun];// O günün iş emri listesini döndür
            }
        }

        public void Ekle(IsEmri isEmri) // İş emrini çizelgeye ekle
        {
            // İş emrinin planlandığı günü al
            var gun = DateOnly.FromDateTime(isEmri.PlanlananTarih); // DateOnly kullanarak sadece tarihi al
            // Indexer'ı kullanarak o günün listesine ekle
            this[gun].Add(isEmri);
        }
    }
}