namespace UstaPlatform.Domain
{
    // Hizmet veren uzman
    public class Usta
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string AdSoyad { get; set; }
        public string UzmanlikAlani { get; set; } // Örn: "Tesisat", "Elektrik"
        public int Puan { get; set; } = 5; // Varsayılan puan
        public int MevcutIsYuku { get; set; } = 0; // O an kaç işi olduğu
        public Cizelge GunlukCizelge { get; set; } = new Cizelge(); // Ustanın günlük iş çizelgesi
        public Rota GunlukRota { get; set; } = new Rota(); // Ustanın günlük rotası
    }
}