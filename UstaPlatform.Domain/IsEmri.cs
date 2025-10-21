using System.Collections.Generic; // EKLENDİ

namespace UstaPlatform.Domain
{
    // Onaylanmış ve ustaya atanmış iş
    public class IsEmri
    {
        public Guid Id { get; init; } = Guid.NewGuid(); // İş emri ID'si
        public Talep KaynakTalep { get; set; } // Hangi talepten geldiği
        public Usta AtananUsta { get; set; }
        public Vatandas Vatandas { get; set; }
        public DateTime PlanlananTarih { get; set; } // İşin yapılacağı tarih
        public decimal Fiyat { get; set; } // Fiyat motoru bunu hesaplayacak

        // --- YENİ EKLENEN ALAN ---
        // Fiyat dökümünü ("Temel Ücret: 200 TL" vb.) saklamak için eklendi.
        public List<string> FiyatDetaylari { get; set; } = new List<string>();

        public (int X, int Y) Konum { get; set; }
        public string Durum { get; set; } = "Planlandı"; // Örn: "Planlandı", "Tamamlandı"
    }
}