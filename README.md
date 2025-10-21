# 🧰 UstaPlatform - Şehrin Uzmanlık Platformu

  
Amacı, şehirdeki vatandaşların tesisatçı, elektrikçi gibi ustalara kolayca ulaşabilmesini sağlamak, fiyatları dinamik hesaplamak ve sistemi eklentilerle genişletebilmektir.  
Kısaca: "Vatandaş talep açar, sistem doğru ustayı bulur ve fiyatı akıllı şekilde hesaplar."

---

## 🎯 Projenin Amacı
Arcadia şehrinde belediyenin sisteminin çökmesi nedeniyle vatandaşlar ustalara ulaşamıyor.  
Bu proje, vatandaş ile ustayı **otomatik olarak eşleştiren**, işi planlayan, fiyatı hesaplayan ve **ek kurallar eklenebilen** bir platform oluşturmayı amaçlıyor.

---

## 🏗️ Proje Yapısı (Katmanlı Mimari)


| Katman | Açıklama |
|--------|-----------|
| **UstaPlatform.Domain** | Sistemin kalbi. Burada `Usta`, `Vatandaş`, `Talep`, `İşEmri`, `Rota`, `Çizelge` gibi sınıflar yer alır. Her sınıfın tek sorumluluğu vardır. |
| **UstaPlatform.Pricing** | Fiyat hesaplamaları bu katmanda yapılır. `IPricingRule` arayüzü ve ondan türeyen kurallar (örneğin HaftaSonuEkUcreti) burada bulunur. |
| **UstaPlatform.Infrastructure** | Depolama, yardımcı sınıflar (`GeoHelper`, `MoneyFormatter`, `Guard`) burada tutulur. Uygulamanın temel altyapısıdır. |
| **UstaPlatform.App** | Ana uygulama (Console veya WinForms). Kullanıcı burada uygulamayı başlatır, verilerle etkileşim kurar. |

Bu yapıda her katman **kendi işini yapar**, böylece kod daha düzenli ve yönetilebilir olur.

---


## 🧠 Tasarım Kararları (SOLID Uygulamaları)

Bu projede **SOLID prensipleri** özellikle vurgulandı.  
Her ilkenin projedeki karşılığını aşağıda anlattım:

1. **S (Single Responsibility - Tek Sorumluluk):**  
   Her sınıf sadece bir işi yapıyor.  
   Örneğin `Usta` sınıfı sadece ustayla ilgili bilgileri tutuyor, fiyat hesaplamıyor.

2. **O (Open/Closed - Açık/Kapalı):**  
   Sistem yeni fiyat kuralı eklemeye **açık**, ama mevcut kodu değiştirmeye **kapalı**.  
   Yani yeni bir DLL ekleyerek sistemi genişletebiliyoruz.

3. **L (Liskov Substitution):**  
   Her alt sınıf (örneğin `HaftaSonuEkUcretiKurali`), `IPricingRule` arayüzü gibi davranıyor.  
   Kod, hangi kural olduğunu bilmeden çalışabiliyor.

4. **I (Interface Segregation):**  
   Her arayüz sadece gerekli metotları içeriyor. Gereksiz yük yok.

5. **D (Dependency Inversion):**  
   Üst katmanlar somut sınıflara değil, **arayüzlere** bağımlı.  
   Örneğin `PricingEngine`, `IPricingRule`’u kullanıyor; hangi kural yüklenecekse dinamik olarak geliyor.

---

## 🔌 Plug-in (Eklenti) Mimarisi Nasıl Çalışır?

Sistemin dinamik olmasını bu yapı sağlıyor.

### 🔹 Temel fikir:
Yeni bir fiyatlandırma kuralı eklemek istiyorum ama **ana kodu değiştirmeden**.  
Bunu yapmak için **DLL (Dynamic Link Library)** yapısını kullandım.

### 🔹 Nasıl çalışıyor:
1. `IPricingRule` adında bir arayüz tanımladım.  
   Her fiyat kuralı bu arayüzü uygular.
2. `PricingEngine` sınıfı, uygulama açıldığında bir klasörü (örneğin `/Plugins`) tarar.  
3. Bu klasördeki her `.dll` dosyasını `Reflection` ile yükler.  
4. DLL içinde `IPricingRule` arayüzünü uygulayan sınıfları bulur.  
5. Tüm kuralları sırayla çalıştırarak toplam fiyatı hesaplar.

Örneğin:
- Temel fiyat: 100₺  
- `HaftaSonuEkUcreti.dll`: +20₺  
- `AcilCagri.dll`: +30₺  

**Toplam = 150₺**

Kodun hiçbir yerine dokunmadan, sadece klasöre yeni DLL atarak fiyat hesaplaması değiştirilebiliyor.  
İşte bu **Açık/Kapalı prensibinin** canlı hali.

---

## 🧩 Kullanılan C# Özellikleri


- `init-only` özellikleri → Sadece ilk atamada değer verilebiliyor (ID, kayıt zamanı gibi).
- `Indexer` (Dizinleyici) → `Schedule[DateOnly]` ile doğrudan o güne ait işleri alabiliyoruz.
- `IEnumerable` → `Route` sınıfı üzerinde foreach döngüsüyle gezinebiliyoruz.
- `Static` yardımcı sınıflar → `GeoHelper`, `MoneyFormatter`, `Guard`.
- Nesne ve koleksiyon başlatıcıları → Kısa ve temiz kod.

---

---

---

## 👨‍💻 Geliştirici Yorumu yaparsak
Bu projede en çok **Plug-in mimarisi** üzerinde uğraştım.  
DLL’leri dinamik olarak taramak ve `Reflection` ile bulmak başlangıçta zordu ama çalışınca çok tatmin edici oldu.  
Sonuçta kodu hiç değiştirmeden sisteme yeni kurallar ekleyebilen, gerçek dünyada işe yarar bir yapı oluşturmuş oldum.

---

## 📄 Kısaca Özet geçersek
- Katmanlı mimari ✅  
- SOLID prensipleri ✅  
- Plug-in sistemi ✅  
- Genişletilebilir yapı ✅  
- Açıklayıcı kod ve dokümantasyon ✅  

Bu README, projeyi inceleyen herkesin sistemin nasıl kurulduğunu, neden bu şekilde tasarlandığını ve nasıl çalıştığını kolayca anlamasını sağlar.
