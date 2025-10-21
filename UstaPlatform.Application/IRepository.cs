using UstaPlatform.Domain;

namespace UstaPlatform.Application
{
    // GEREKLİLİK 3.A.2 (SRP) ve 3.A.3 (DIP):
    // Veri erişim işini ayırıyoruz.
    // Üst katmanlar (App) somut 'InMemoryUstaRepository' sınıfını değil,
    // bu arayüzleri bilir.
    public interface IRepository<T> where T : class
    {
        T GetById(Guid id); // ID'ye göre tekil nesne getirir
        IEnumerable<T> GetAll(); //Tüm nesneleri getirir
        void Add(T entity); // Yeni nesne ekler
    }

    // Ustaya özel metotlar gerekirse buraya eklenebilir
    public interface IUstaRepository : IRepository<Usta> // Usta için genel depo işlevselliği
    {
        Usta GetBestAvailable(string uzmanlikAlani);    // Belirtilen uzmanlık alanında en uygun ustayı getirir
    }

    public interface IVatandasRepository : IRepository<Vatandas> { } // Vatandaş için genel depo işlevselliği
    public interface IWorkOrderRepository : IRepository<IsEmri> { } // İş emri için genel depo işlevselliği
}