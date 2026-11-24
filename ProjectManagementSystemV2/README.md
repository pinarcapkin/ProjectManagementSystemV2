# Proje Yönetim Sistemi (Project Management System)

Üniversite staj kapsamında geliştirilen, ASP.NET Core Web API tabanlı bir proje ve görev takip sistemi backend'i ile HTML/JavaScript frontend arayüzü.

## Kullanılan Teknolojiler

- **ASP.NET Core Web API** (.NET 8)
- **Entity Framework Core** (SQLite veritabanı)
- **JWT (JSON Web Token)** ile kimlik doğrulama
- **BCrypt** ile şifre hash'leme
- **FluentValidation** ile veri doğrulama
- **Swagger / OpenAPI** ile API dokümantasyonu ve test arayüzü
- **HTML, CSS ve Vanilla JavaScript** (Giriş, Projelerim, Proje Detay ve Görevlerim ekranları)

## Proje Mimarisi

Proje, Repository Pattern ve Service Layer katmanlarıyla yapılandırılmıştır:

- **Controllers/** — API uç noktaları (endpoint'ler)
- **Services/** — İş kuralları (business logic)
- **Repositories/** — Veritabanı erişim katmanı
- **Entities/** — Veri modelleri (Users, Roles, Projects, Tasks, Comments, vb.)
- **Entities/DTOs/** — İstek/cevap veri transfer nesneleri
- **Middleware/** — Global hata yönetimi
- **Data/** — AppDbContext ve veritabanı yapılandırması

## Kurulum

### Gereksinimler
- .NET 8 SDK
- (Opsiyonel) JetBrains Rider veya Visual Studio

### Adımlar

1. Projeyi klonlayın / indirin
2. Terminal üzerinden proje klasörüne gidin
3. Gerekli paketleri yükleyin:
   `dotnet restore`
4. Veritabanını oluşturun (migration'ları uygulayın):
   `dotnet ef database update`
5. Uygulamayı çalıştırın:
   `dotnet run`
6. Tarayıcıda Swagger arayüzüne gidin (genellikle `http://localhost:5094/swagger`) ve arayüz üzerinden veya statik sunucuyla frontend sayfalarına (`index.html`, `Projelerim`, `Proje Detay`, `Görevlerim`) erişin.

## Test Kullanıcısı

Veritabanı ilk kurulduğunda otomatik olarak bir Admin kullanıcı oluşturulur:

- **E-posta:** admin@sistem.com
- **Şifre:** Admin123!

Test hesabı:
- **E-posta:** zeynep.kaya2026@mail.com

## Swagger Üzerinden Kimlik Doğrulama

1. `POST /api/auth/login` ile giriş yapıp bir JWT token alın
2. Swagger sayfasının üstündeki **Authorize** butonuna tıklayın
3. `Bearer <token>` formatında (aralarında tek boşluk) girin
4. Artık korumalı tüm endpoint'leri test edebilirsiniz

## Ana Modüller

- **Auth** — Kayıt olma, giriş yapma
- **Users & Roles** — Kullanıcı yönetimi, rol atama/kaldırma, aktif/pasif yapma
- **Projects** — Proje CRUD işlemleri, sayfalama, filtreleme, sıralama, arşivleme, ekip üyeliği
- **Tasks** — Görev CRUD işlemleri, atama, durum takibi (`Done` durumunda `CompletedAt` yönetimi), değişiklik geçmişi (`TaskHistories`)
- **Comments** — Görevlere yorum ekleme/listeleme/güncelleme/silme
- **TaskTimeLogs** — Görevler üzerinde harcanan zamanın kaydı ve toplam saat hesaplama

## Notlar

- Kullanıcı silme ve çıkarma işlemleri fiziksel değil, "soft delete" (`IsActive` / `IsDeleted` / `IsArchived`) mantığıyla yapılır.
- Tüm korumalı endpoint'ler JWT Bearer token gerektirir.
- Admin rolüne sahip kullanıcılar ek yetkilere (kullanıcı/rol yönetimi gibi) sahiptir.