namespace UstaPlatform.Domain
{
    // Vatandaşın açtığı iş talebi
    public class Talep
    {
        // init-only özellik: Sadece oluşturulurken değer atanabilir
        public Guid Id { get; init; } = Guid.NewGuid(); // Benzersiz talep ID'si
        public DateTime KayitZamani { get; init; } = DateTime.UtcNow; // Kayıt zamanı
        public Vatandas TalepEden { get; set; }
        public string Aciklama { get; set; }
        public string GerekenUzmanlik { get; set; } // Usta ile eşleşmek için
        public (int X, int Y) Konum { get; set; }
        public bool AcilMi { get; set; } = false;
    }
}