# 🎵 ASP.NET Core 6.0 ve JWT ile Bepop Müzik Uygulaması Sitesi
Bu repository, M&Y Yazılım Akademi bünyesinde yaptığım on ikinci proje olan ASP.NET Core 6.0 ve JWT ile Bepop Müzik Uygulaması Sitesi projesini içermektedir. Bu eğitimde bana yol gösteren Murat Yücedağ'a çok teşekkür ederim.

Bu proje, ASP.NET Core 6.0 ve JWT (JSON Web Token) Authentication kullanılarak geliştirilmiş, 6 katmanlı mimari tababnında modern bir müzik dinleme platformudur; bu sayede temiz kod, anlaşılabilirlik ve genişletilebilirlik ön planda tutulmuştur. JWT tabanlı kullanıcı yönetimi ile güvenli bir şekilde kullanıcı kayıt, giriş ve çıkış işlemleri sağlanmaktadır; böylece her kullanıcı yalnızca kendi verilerine erişebilir. Projede ayrıca paket yapısı uygulanmıştır; kullanıcılar sahip oldukları pakete göre erişebilecekleri şarkı ve sanatçılara sınırlandırılır, bu da farklı abonelik seviyeleri için esnek bir yapı sunar. Kullanıcılar, paketlerine uygun şarkıları keşfederken, şarkı ve sanatçı seçimi kolaylaştırılmıştır; filtreleme ve listeleme mekanizmaları sayesinde kullanıcı deneyimi akıcı ve sezgiseldir. Projenin en dikkat çekici yönü ise ML.NET tabanlı öneri sistemidir; kullanıcıların geçmiş dinleme alışkanlıkları analiz edilerek, onlara en uygun şarkı ve sanatçı önerileri sunulur. Bu sistem, klasik popüler müzik sıralamalarının ötesine geçerek, kişiselleştirilmiş ve dinamik bir müzik deneyimi yaratmaktadır.

Veri tabanı olarak Microsoft SQL Server üzerinde ilişkisel tablolar tasarlanmış ve Paketler, Şarkılar, Kullanıcılar ve Kullanıcı - Şarkı Geçmişi gibi temel entity’ler için dinamik veri yapıları oluşturulmuştur. Bu sayede proje sadece bir demo değil, gerçek bir sektörel uygulamaya dönüştürülebilecek nitelikte güçlü bir temel kazanmıştır. Projede eksiklikler muhakkak vardır. Bu bir eğitim projesidir.

---

🚀 Proje Özellikleri

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
Bepop teması üzerine modern, responsive ve kullanıcı dostu bir arayüz tasarlanmıştır.
Şarkı oynatma butonları, tür filtreleme ve hızlı erişim özellikleri ile kullanıcı deneyimi optimize edilmiştir.

---

### 🚀 Kullandığım Teknolojiler
- 💻 ASP.NET Core 9.0 → Projenin backend kısmı, modern .NET Core mimarisiyle geliştirildi.
- 🗂 CQRS (Command Query Responsibility Segregation) → Okuma (Query) ve yazma (Command) işlemleri ayrıştırıldı, temiz kod ve sürdürülebilirlik sağlandı.
- 📐 Tek Katmanlı Yapı → Tek katman üzerinde klasörler ile ayrılmış dosya düzeni sağlandı.
- 🗄️ MS SQL Server → Entity'ler ve İlişkili Tablolar MS SQL Server üzerinde düzenlendi.
- 🖼 ViewComponent → Tekrarlayan UI parçalarını yönetmek için kullanıldı.
- 🎨 HTML5, CSS3, JavaScript, Bootstrap → Arayüz tasarımı.
- 🌍 Hugging Face – Helsinki NLP → Türkçe ↔ İngilizce otomatik çeviri için kullanıldı.
- 🛢 RapidAPI Entegrasyonları:
- ⛽ Yakıt Fiyatları API → Türkiye’deki benzin, motorin ve LPG fiyatları.
- ✈️ Havalimanları API → Türkiye’deki havalimanlarının listelenmesi.
- 📏 Havalimanları Arası Mesafe API → İki havalimanı arasındaki mesafeyi hesaplama.
- 🤖 Chatbot API (Mesaj Yanıtı) → Müşterilerin sorularını cevaplayan basit yapay zekâ destekli sohbet botu.
- 🚗 Chatbot API (Araç Öneri Asistanı) → Müşterilere araç önerileri yapan araç öneri asistanı.

🖥 Backend:
- ⚡ ASP.NET Core 6.0
- 🗄 Entity Framework Core
- 📊 LINQ ile veri işleme

🌐 Frontend:
- 📝 Razor sayfaları
- 💻 HTML5, CSS3, JavaScript

🎨 AdminLTE arayüz kütüphanesi
🗃 Veritabanı:
🛢 SQL Server, kullanıcı ve şarkı verilerinin yönetimi için kullanılmıştır
🔐 Authentication:
🔑 JWT (JSON Web Token) ile güvenli kullanıcı kimlik doğrulama
🤖 Machine Learning:
🧠 ML.NET kullanılarak, kullanıcıların geçmiş dinleme alışkanlıklarına dayalı öneri sistemi geliştirilmiştir

Projede genel anlamda 2 bölüm bulunmaktadır.<br>
- Ana Sayfa: Burada kullanıcı, araç kiralam sitesinin detaylarını görmektedir. İstediği takdirde uygun araç modeli ve tarihe göre rezervasyonunu yapabilir. Bize Ulaşın bölümünden de firmaya mesaj gönderebilir.
- Admin Paneli: Burada admin tarafından hakkında, rezervasyonlar, arabalar, hava limanları gibi bölümlerin CRUD işlemleri yapılmaktadır. Dashboard bölümünde ise bazı istatistikler yer almaktadır.

---

## :arrow_forward: Projeden Ekran Görüntüleri

### :triangular_flag_on_post: Ana Sayfa
<div align="center">
  <img src="https://github.com/melihcolak0/CqrsCentalRentACar/blob/1037666abed69f81bef1604e4304bf57ec771770/ss3/screencapture-localhost-7100-Default-Index-2025-08-26-15_47_20.png" alt="image alt">
</div>
