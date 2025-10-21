using UstaPlatform.Domain;

namespace UstaPlatform.Application
{
    // GEREKLİLİK 4: Plug-in Mimarisi
    // Bu arayüz, bizim "fiş" standardımızdır.
    // Dışarıdan eklenen her DLL (plug-in), bu arayüzü uygulayan
    // bir sınıf içermek zorundadır.
    public interface IPricingRule
    {
        string KuralAdi { get; }

        // Mevcut fiyatı alır, kuralı uygular ve yeni fiyatı döner.
        decimal Hesapla(IsEmri isEmri, decimal mevcutFiyat);
    }
}