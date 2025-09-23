# 🎵 ASP.NET Core 6.0 ve JWT ile Bepop Müzik Uygulaması Sitesi
Bu repository, M&Y Yazılım Akademi bünyesinde yaptığım on ikinci proje olan ASP.NET Core 6.0 ve JWT ile Bepop Müzik Uygulaması Sitesi projesini içermektedir. Bu eğitimde bana yol gösteren Murat Yücedağ'a çok teşekkür ederim.

Bu proje, ASP.NET Core 6.0 ve JWT (JSON Web Token) Authentication kullanılarak geliştirilmiş, 6 katmanlı mimari tababnında modern bir müzik dinleme platformudur; bu sayede temiz kod, anlaşılabilirlik ve genişletilebilirlik ön planda tutulmuştur. JWT tabanlı kullanıcı yönetimi ile güvenli bir şekilde kullanıcı kayıt, giriş ve çıkış işlemleri sağlanmaktadır; böylece her kullanıcı yalnızca kendi verilerine erişebilir. Projede ayrıca paket yapısı uygulanmıştır; kullanıcılar sahip oldukları pakete göre erişebilecekleri şarkı ve sanatçılara sınırlandırılır, bu da farklı abonelik seviyeleri için esnek bir yapı sunar. Kullanıcılar, paketlerine uygun şarkıları keşfederken, şarkı ve sanatçı seçimi kolaylaştırılmıştır; filtreleme ve listeleme mekanizmaları sayesinde kullanıcı deneyimi akıcı ve sezgiseldir. Projenin en dikkat çekici yönü ise ML.NET tabanlı öneri sistemidir; kullanıcıların geçmiş dinleme alışkanlıkları analiz edilerek, onlara en uygun şarkı ve sanatçı önerileri sunulur. Bu sistem, klasik popüler müzik sıralamalarının ötesine geçerek, kişiselleştirilmiş ve dinamik bir müzik deneyimi yaratmaktadır.

Veri tabanı olarak Microsoft SQL Server üzerinde ilişkisel tablolar tasarlanmış ve Paketler, Şarkılar, Kullanıcılar ve Kullanıcı - Şarkı Geçmişi gibi temel entity’ler için dinamik veri yapıları oluşturulmuştur. Bu sayede proje sadece bir demo değil, gerçek bir sektörel uygulamaya dönüştürülebilecek nitelikte güçlü bir temel kazanmıştır. Projede eksiklikler muhakkak vardır. Bu bir eğitim projesidir.

### 📂 Proje Yapısı
- Jwt.PresentationLayer → MVC Controller & View katmanı
- Jwt.ApiLayer → RESTful API uç noktaları, dış sistemlerle entegrasyon
- Jwt.DtoLayer → Veri transfer objeleri (DTO)
- Jwt.BusinessLayer → İş kuralları ve ML model entegrasyonu
- Jwt.DataAccessLayer → Entity Framework Core ile veri erişim katmanı
- Jwt.EntityLayer → Entity tanımları

---

### 🌟 Proje Özellikleri

🔐 JWT Tabanlı Kimlik Doğrulama
- Kullanıcıların kayıt, giriş ve çıkış işlemleri JWT (JSON Web Token) ile güvenli bir şekilde yönetilmektedir. Bu sayede kullanıcılar sadece kendi verilerine erişebilir ve yetkisiz girişler engellenir.

🎵 Şarkı ve Sanatçı Yönetimi
- Tüm şarkılar ve sanatçılar veritabanından dinamik olarak listelenir.
- Kullanıcılar şarkıları tür, sanatçı veya paket seviyesine göre filtreleyebilir.
- Her şarkının detay sayfasında kapak görseli, sanatçı bilgisi, albüm ve diğer meta veriler gösterilir, böylece kullanıcı deneyimi zenginleştirilir.

📈 Dinlenme Takibi
- Kullanıcı bir şarkıyı oynattığında, şarkının dinlenme sayısı otomatik olarak güncellenir.
- Tüm kullanıcı-şarkı etkileşimleri UserSongHistory tablosunda tutulur, böylece geçmiş dinleme verileri kayıt altında olur.

🤖 Öneri Sistemi (Machine Learning)
- ML.NET kullanılarak, kullanıcıların geçmiş dinleme alışkanlıkları analiz edilir ve kişiselleştirilmiş şarkı önerileri oluşturulur.
- Kullanıcıya, benzer müzik zevkine sahip diğer kullanıcıların tercihleri de öneri modeline dahil edilir.
- Bu sistem, klasik popüler listelerin ötesinde, tamamen kişisel ve dinamik bir müzik deneyimi sunar.

📦 Paket Sistemi
- Kullanıcılar sahip oldukları pakete göre belirli şarkılara ve sanatçılara erişebilir.
- Örnek paketler: Basic, Premium. Bu yapı, uygulamanın esnekliğini artırır ve farklı abonelik seviyeleri için uyarlanabilir.

🎨 Kullanıcı Arayüzü
- Bepop teması üzerine modern, responsive ve kullanıcı dostu bir arayüz tasarlanmıştır.
- Şarkı oynatma butonları, tür filtreleme ve hızlı erişim özellikleri ile kullanıcı deneyimi optimize edilmiştir.

---

### 🚀 Kullandığım Teknolojiler
- 💻 ASP.NET Core 6.0 → Projenin backend kısmı, modern .NET Core mimarisiyle geliştirildi.
- 📂 N-Katmanlı Yapı → Altı katman üzerinden yönetilebilir ve genişletilebilir bir yapı sağlandı.
- 🗄 Entity Framework Core → Veritabanı işlemleri için kullanıldı.
- 📊 LINQ → Veri sorgulama ve işleme için kullanıldı.
- 🛢 Microsoft SQL Server → Entity’ler ve ilişkili tablolar burada düzenlendi.
- 🖼 ViewComponent → Tekrarlayan UI parçalarının yönetimi için kullanıldı.
- 🎨 HTML5, CSS3, JavaScript, Bootstrap → Modern ve responsive arayüz tasarımı.
- 🔐 JWT Authentication → Güvenli kullanıcı giriş ve kayıt işlemleri için kullanıldı.
- 🤖 ML.NET → Kullanıcıların geçmiş dinleme alışkanlıklarına dayalı öneri sistemi geliştirildi.

<br>
Projede genel anlamda 2 bölüm bulunmaktadır.<br>
- Ana Sayfa: Burada kullanıcı, kayıt olduğu paket seviyesine göre şarkı dinleme, kendi listelerini oluşturma, istediği sanatçının şarkılarını ve detaylarını görüntüleme gibi birçok işlemi yapabilmektedir.<br>
- Admin Paneli: Burada admin tarafından paketler, şarkılar, kullanıcılar, kullanıcı-şarkı geçmişi gibi bölümler ile ilgili CRUD işlemler gerçekleştirilir. Dashboard bölümünde ise bazı istatistikler yer almaktadır.

---

## :arrow_forward: Projeden Ekran Görüntüleri

### :triangular_flag_on_post: Ana Sayfa
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-Starter-Index-2025-09-21-18_58_00.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-Default-2025-09-21-18_49_45.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/c9afd07ef596f7bbb4dce9b26075715e56d26c0e/ss2/screencapture-localhost-7179-Charts-Index-2025-09-22-16_08_08.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/c9afd07ef596f7bbb4dce9b26075715e56d26c0e/ss2/screencapture-localhost-7179-Genres-Index-2025-09-22-16_15_34.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-Genres-GetSongById-75-2025-09-21-18_58_45.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/afcba85fc3b353f2002c367c35fc463b766767c7/ss2/screencapture-localhost-7179-Artists-Index-2025-09-22-15_50_32.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-Artists-GetArtistByName-2025-09-21-19_00_53.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-UpgradePackage-Index-2025-09-22-13_58_33.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-register-Index-2025-09-21-19_01_28.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-Login-Index-2025-09-21-19_01_07.png" alt="image alt">
</div>
<br>

### :triangular_flag_on_post: Admin Paneli
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-AdminDashboard-Index-2025-09-21-19_02_13.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-AdminPackage-Index-2025-09-21-19_02_32.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-AdminPackage-CreatePackage-2025-09-21-19_02_47.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-AdminPackage-UpdatePackage-6-2025-09-21-19_02_57.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/localhost_7179_AdminSong_Index.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-AdminUser-Index-2025-09-21-19_03_56.png" alt="image alt">
</div>
<div align="center">
  <img src="https://github.com/melihcolak0/JwtBepopMusic/blob/b73981baf604b55fca3bf5971cfd0df627468dfe/ss/screencapture-localhost-7179-AdminUserSongHistory-Index-2025-09-21-19_04_06.png" alt="image alt">
</div>
