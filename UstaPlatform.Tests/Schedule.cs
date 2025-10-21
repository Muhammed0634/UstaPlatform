using UstaPlatform.Domain;

namespace UstaPlatform.Tests
{
    public class ScheduleIndexerTests
    {
        [Fact] // Bu, xUnit'e bu metodun bir test olduğunu söyler
        public void Ekle_ve_DizinleyiciIleGetir_DogruGunuDondurmelidir()
        {
            // --- ARRANGE ---
            // Test için gerekli nesneleri hazırla
            var schedule = new Cizelge();
            var testGunu = new DateOnly(2025, 10, 20); // Pazartesi
            var baskaGun = new DateOnly(2025, 10, 21); // Salı

            var isEmri = new IsEmri
            {
                // Test için diğer alanlar önemli değil
                PlanlananTarih = testGunu.ToDateTime(TimeOnly.MinValue)
            };

            // --- ACT ---
            // Test edilecek metodu çalıştır
            schedule.Ekle(isEmri);

            // --- ASSERT ---
            // Sonucun doğruluğunu kontrol et
            Assert.Single(schedule[testGunu]); // Test gününde 1 tane iş emri olmalı
            Assert.Equal("Planlandı", schedule[testGunu][0].Durum); // Eklenen iş emrinin durumu doğru mu?
            Assert.Empty(schedule[baskaGun]); // Başka bir günde hiç iş emri olmamalı
        }
    }
}