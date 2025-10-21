using UstaPlatform.Application;
using UstaPlatform.Domain;

namespace UstaPlatform.Infrastructure
{
    // Bu bizim sahte (In-Memory) veri tabanımız.
    // DIP sayesinde, App projesi bununla değil, IUstaRepository ile konuşur.
    // Yarın SQL'e geçtiğimizde sadece bu sınıfı değiştiririz.
    public class InMemoryUstaRepository : IUstaRepository
    {
        private static readonly List<Usta> _ustalar = new();

        public void Add(Usta entity)
        {
            _ustalar.Add(entity);
        }

        public IEnumerable<Usta> GetAll()
        {
            return _ustalar;
        }

        // Rehberde istenen basit eşleştirme yöntemi
        public Usta GetBestAvailable(string uzmanlikAlani)
        {
            return _ustalar
                .Where(u => u.UzmanlikAlani == uzmanlikAlani)
                .OrderBy(u => u.MevcutIsYuku) // En az iş yükü olanı seç
                .FirstOrDefault();
        }

        public Usta GetById(Guid id)
        {
            return _ustalar.FirstOrDefault(u => u.Id == id);
        }
    }
}