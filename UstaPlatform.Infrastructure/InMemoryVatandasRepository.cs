using UstaPlatform.Application; // IVatandasRepository arayüzü için
using UstaPlatform.Domain;       // Vatandas sınıfı için

namespace UstaPlatform.Infrastructure
{
    // Bu, Vatandas varlıkları için sahte (In-Memory) veri tabanıdır.
    // IVatandasRepository arayüzünü uygular.
    public class InMemoryVatandasRepository : IVatandasRepository
    {
        // Verileri statik bir listede tutuyoruz (demo için)
        private static readonly List<Vatandas> _vatandaslar = new();

        public void Add(Vatandas entity)
        {
            _vatandaslar.Add(entity);
        }

        public IEnumerable<Vatandas> GetAll()
        {
            return _vatandaslar;
        }

        public Vatandas GetById(Guid id)
        {
            return _vatandaslar.FirstOrDefault(v => v.Id == id);
        }
    }
}