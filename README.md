# 🛒 ECommerce Payment API

Bu proje, basit bir e-ticaret ödeme altyapısı sunar. API, ürün yönetimi, sipariş oluşturma ve ödeme işlemleri gibi temel işlevleri içermektedir.

---

## 🚀 Başlarken

### 💾 Veritabanı Bağlantısı

- Projede **MSSQL** veritabanı kullanılmıştır.
- `appsettings.json` dosyasındaki `ConnectionStrings` kısmı kendi **local SQL Server** yapınıza göre güncellenmelidir.

---

### ⚙️ Migration ve Veritabanı Oluşturma

Aşağıdaki komutları çalıştırarak migration işlemlerini gerçekleştirebilirsiniz. Komutlar **Persistence** klasöründen çalıştırılmalıdır:

```bash
dotnet ef --startup-project ../ECommercePaymentApi migrations add InitialCreate -c AppDbContext
dotnet ef --startup-project ../ECommercePaymentApi database update -c AppDbContext
```

---

## 📚 API Dokümantasyonu

Proje içinde **Swagger** desteği mevcuttur.

- Swagger UI’ye şu adresten erişebilirsiniz:  
  [http://localhost:{port}/swagger/index.html](http://localhost:{port}/swagger/index.html)

---

## ❤️ Health Checks

Uygulamada **Health Check** mekanizması entegre edilmiştir.  
SQL Server servisi gibi bağımlılıkların **sağlıklı (healthy)** veya **hatalı (unhealthy)** durumu takip edilebilir.

- Health kontrol URL'si: `appsettings.json` dosyasındaki `HealthCheckEndpoint` ayarından yapılandırılır.
- Kendi local portunuza göre `appsettings.json` içeriği güncellenmelidir.

### 🔍 Endpoints

- **JSON Health Endpoint:** `/health`  
- **UI Health Endpoint:** `/health-ui`

---

## 📌 Notlar

- .NET 9 kullanılmaktadır. Uyumlu SDK’nın kurulu olduğundan emin olun.
- `HttpClient`'lar Polly ile retry policy destekleyecek şekilde yapılandırılmıştır.
