namespace UstaPlatform.Domain
{
    // Hizmet talep eden kişi
    public class Vatandas
    {
        public Guid Id { get; init; } = Guid.NewGuid(); // Sadece okunur, nesne oluşturulurken atanır
        public string AdSoyad { get; set; }   
        public string Adres { get; set; }
        public (int X, int Y) Konum { get; set; }
    }
}