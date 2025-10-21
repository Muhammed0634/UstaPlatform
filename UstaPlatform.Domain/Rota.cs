using System.Collections;
using System.Collections.Generic; // EKLENDİ
using System.Linq; // EKLENDİ

namespace UstaPlatform.Domain
{
    // Bir uzmanın günlük ziyaret edeceği adreslerin sırası
    // GEREKLİLİK 3.B.4: Özel IEnumerable Koleksiyonu
    public class Rota : IEnumerable<(int X, int Y)> // (X,Y) koordinat çiftlerinden oluşan rota
    {
        private readonly List<(int X, int Y)> _duraklar = new(); // Rota durakları

        // GEREKLİLİK 3.B.4 (Devamı): Koleksiyon başlatıcıyı desteklemek için
        // new Rota { {10, 20}, {30, 40} } şeklinde yazabilmemizi sağlar.
        public void Add(int X, int Y)
        {
            _duraklar.Add((X, Y));
        }

        public void Add(IsEmri isEmri) // İş emrinden konumu ekleme
        {
            _duraklar.Add(isEmri.Konum);
        }

        // IEnumerable arayüzünün uygulanması
        public IEnumerator<(int X, int Y)> GetEnumerator() // Rota üzerindeki durakları döner
        {
            // Rehber "basit tutun" dedi.
            // Sadece X koordinatına göre sıralayıp dönüyoruz (basit rota optimizasyonu)
            return _duraklar.OrderBy(d => d.X).ThenBy(d => d.Y).GetEnumerator(); // X'e göre sıralı
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // --- YENİ EKLENEN KODLAR (MESAFE HESAPLAMA) ---

        /// <summary>
        /// İki nokta arasındaki Öklid mesafesini hesaplar.
        /// </summary>
        private static double MesafeHesapla((int X, int Y) a, (int X, int Y) b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        /// <summary>
        /// Sıralanmış rotadaki tüm duraklar arasındaki toplam mesafeyi verir.
        /// </summary>
        public double HesaplaToplamMesafe()
        {
            // GetEnumerator() metodumuzun sağladığı sıralı listeyi alıyoruz.
            var siraliDuraklar = this.ToList();

            if (siraliDuraklar.Count < 2)
            {
                return 0; // Mesafe yok
            }

            double toplamMesafe = 0;

            // Duraklar arasında gez (0->1, 1->2, 2->3 ...)
            for (int i = 0; i < siraliDuraklar.Count - 1; i++)
            {
                var mevcutDurak = siraliDuraklar[i];
                var sonrakiDurak = siraliDuraklar[i + 1];
                toplamMesafe += MesafeHesapla(mevcutDurak, sonrakiDurak);
            }

            return toplamMesafe;
        }
    }
}