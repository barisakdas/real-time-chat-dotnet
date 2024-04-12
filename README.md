# REAL TIME CHATTING WITH DOTNET

## SignalR Nedir?
SignalR, ASP.NET geliştiricilerine uygulamalara gerçek zamanlı web işlevselliği eklemeyi kolaylaştıran bir kütüphanedir. Sunucu kodunun, istemcinin yeni veri istemesini beklemeden bağlı istemcilere anında içerik gönderebilmesini sağlar.

SignalR’da Clients nesnesi, sunucu tarafından client’lara mesaj göndermek için kullanılır ve çeşitli özellikler içerir. İşte All, Others ve Caller özelliklerinin açıklamaları:

* Clients.All: Hub’a bağlı olan `tüm client’lara` mesaj göndermek için kullanılır. Bu, bir sunucu metodunun tüm bağlı kullanıcılara aynı anda veri göndermesini sağlar.
* Clients.Others: Mesajı gönderen client hariç, hub’a bağlı olan `diğer tüm client’lara` mesaj göndermek için kullanılır. Yani, bir kullanıcı bir mesaj gönderdiğinde, bu mesaj kendisine geri gönderilmez, ancak diğer tüm bağlı kullanıcılara gönderilir.
* Clients.Caller: Yalnızca mesajı gönderen client’a, yani `çağrıyı yapan client’a` mesaj göndermek için kullanılır. Bu, genellikle bir kullanıcının kendi eylemine yanıt olarak geri bildirim sağlamak için kullanılır.

Bu özellikler, bir SignalR uygulamasında farklı senaryolara göre mesajlaşmayı özelleştirmek için kullanılır. Örneğin, bir oyun uygulamasında bir oyuncunun hamlesini tüm diğer oyunculara göndermek için Clients.Others kullanılabilirken, bir form gönderildiğinde yalnızca gönderen kullanıcıya başarı mesajı göndermek için Clients.Caller kullanılabilir.

Clients nesnesi üzerinde başka amaçlar için kullanılan metotlar da mevcuttur.
* Clients.AllExcept: Bu metod, belirtilen bağlantı kimliklerine (connection IDs) sahip client’lar hariç olmak üzere, hub’a bağlı tüm client’lara mesaj göndermek için kullanılır. Örneğin, bir kullanıcının kendi eylemi hakkında diğer tüm kullanıcılara bilgi vermek istediğinde, ancak bu bilginin kendisine geri gönderilmesini istemediğinde kullanılır.
```csharp
public async Task SendMessageToAllExcept(string message, string[] excludedConnectionIds)
    => await Clients.AllExcept(excludedConnectionIds).SendAsync("ReceiveMessage", message);
```

* Clients.GroupExcept: Bu metod, belirli bir gruptaki belirtilen bağlantı kimliklerine sahip client’lar hariç, gruptaki tüm client’lara mesaj göndermek için kullanılır. Örneğin, bir grup içindeki bir olay hakkında gruptaki diğer üyelere bilgi vermek istediğinizde, ancak belirli üyelerin bu bilgiyi almamasını istediğinizde kullanılır.
```csharp
public async Task SendMessageToGroupExcept(string groupId, string message, string[] excludedConnectionIds)
    => await Clients.GroupExcept(groupId, excludedConnectionIds).SendAsync("ReceiveMessage", message);
```
Bu metodlar, mesaj gönderimini daha hedefli hale getirerek, belirli senaryolarda gereksiz mesaj alışverişini önlemek için kullanışlıdır. Örneğin, bir oyun uygulamasında bir oyuncunun hamlesini tüm diğer oyunculara göndermek isteyebilirsiniz, ancak hamleyi yapan oyuncunun bu bilgiyi tekrar almasını istemeyebilirsiniz. Bu durumda Clients.AllExcept kullanılır. Benzer şekilde, bir grup sohbetinde bir kullanıcı bir mesaj gönderdiğinde, bu mesajın gruptaki diğer tüm kullanıcılara gitmesini, ancak belirli kullanıcılara gitmemesini isteyebilirsiniz; bu durumda Clients.GroupExcept kullanılır.

* Clients.User(userId): Belirli bir kullanıcıya mesaj gönderir.
```csharp
public async Task SendMessageToUser(string userId, string message)
    => await Clients.User(userId).SendAsync("ReceiveMessage", message);
```

* Clients.Users(userIds): Belirli kullanıcılara mesaj gönderir.
```csharp
public async Task SendMessageToUsers(string[] userIds, string message)
    => await Clients.Users(userIds).SendAsync("ReceiveMessage", message);
```

* Clients.Group(groupId): Belirli bir gruptaki tüm client’lara mesaj gönderir.
```csharp
public async Task SendMessageToGroup(string groupId, string message)
    => await Clients.Group(groupId).SendAsync("ReceiveMessage", message);
```

* Clients.Groups(groupIds): Belirli gruplardaki tüm client’lara mesaj gönderir.
```csharp
public async Task SendMessageToGroups(string[] groupIds, string message)
    => await Clients.Groups(groupIds).SendAsync("ReceiveMessage", message);
```

* Clients.OthersInGroup: SignalR’da Clients.OthersInGroup metodu, belirli bir gruptaki çağrıyı yapan client hariç diğer tüm client’lara mesaj göndermek için kullanılır. Bu, bir kullanıcının gruptaki diğer kullanıcılara bir mesaj göndermesini, ancak bu mesajın kendisine geri gönderilmemesini istediğinde kullanışlıdır.

Örneğin, bir oyun uygulamasında bir oyuncu bir hamle yaptığında, bu hamlenin diğer oyunculara bildirilmesini, ancak hamleyi yapan oyuncuya bildirilmemesini isteyebilirsiniz. Bu durumda Clients.OthersInGroup metodunu kullanabilirsiniz:
```csharp
public async Task SendMoveToOthersInGroup(string userId, string message)
    => wait Clients.OthersInGroup(groupName).SendAsync("ReceiveMove", move);
```

* Clients.Client(connectionId): Belirli bir bağlantı kimliğine sahip client’a mesaj gönderir.
```csharp
public async Task SendMessageToConnection(string connectionId, string message)
    => await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
```

* Clients.Clients(connectionIds): Belirli bağlantı kimliklerine sahip client’lara mesaj gönderir.
```csharp
public async Task SendMessageToConnections(string[] connectionIds, string message)
    => await Clients.Clients(connectionIds).SendAsync("ReceiveMessage", message);
```

## Server Nedir?
Server, istemcilerden gelen isteklere yanıt veren ve genellikle veri tabanı, dosya paylaşımı veya uygulama hizmetleri gibi kaynakları barındıran bir bilgisayardır.
SignalR kütüphanesinde serverler Hub adı verilen sınıftan kalıtım alarak sağlanır.

## Hub Nedir?
Hub, SignalR’da bağlı istemcilerin sunucudaki yöntemleri çağırmasına olanak tanıyan ve sunucu ile istemciler arasındaki iletişimi yöneten bir bileşendir. Hub’lar, istemcilerden ve sunucudan çağrılan yöntemleri tanımlar.
Bir hub oluştururken mutlaka ilgili sınıfın bu sınıftan kalıtım alması gereklidir.

```csharp
using Microsoft.AspNetCore.SignalR;

/// <summary>
/// ChatHub, SignalR altyapısını kullanarak gerçek zamanlı mesajlaşma işlevselliği sağlayan bir hub sınıfıdır.
/// İstemcilerin sunucuya bağlanmasını, mesaj göndermesini ve almasını sağlar.
/// Ayrıca, istemcileri gruplara ekleyip çıkarmak ve gelen mesajları işleyip geri göndermek gibi işlemleri destekler.
/// </summary>
public class ChatHub : Hub
{
    /* Metotların tanımlanacağı alan*/
}
```

## Client Nedir?
Client, sunucu tarafından sağlanan hizmetleri veya kaynakları kullanan bir uygulama veya cihazdır. Örneğin, bir web tarayıcısı bir web sunucusundan sayfaları isteyen bir istemcidir. Clientların sadece web projeleri olması gerekmez. Bir workerspace, console uygulaması veya zamarin ile yazılan bir mobil uygulama da client olabilir.
Javascript ile hali hazırdaki bir servera bağlantı aşağıdaki gibi yapılır.

```javascript
// İlk olarak projemize SignalR kütüphanesini CDN olarak ekliyoruz.
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/3.1.9/signalr.min.js"></script>

// bir bağlantı oluşturuyoruz.
const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("https://localhost:7000/chathub")  // Server üzerindeki tanımlanmış Hub urli.
    .withAutomaticReconnect([1000, 2000, 3000, 5000, 10000])   // Bağlantı sağlandıktan sonra koparsa dizi içerisinde belirlediğimiz periyotta tekrar bağlantı dener. Örneğin ilk deneme için 1 saniye, ikinci deneme için 2 saniye, üçüncü deneme için 3 saniye bekler gibi.
    .build();

    // Hub içerisindeki metoda bağlantı sağlanıyor.
    connection.on("ReceiveMessage", message => {
        /*Gelen mesaj için yapılacak işlemler*/
        console.log(message)
    });
```

Eğer ki client üzerinden serverdaki bir metodu tetiklemek istiyorsanız şu şekilde yapabilirsiniz:
```javascript
// Server'daki 'SendMessage' metodunu tetiklemeyi sağlıyoruz. Bu metot tetiklendikten sonra ilgili kararları artık Hub tarafı verecek.
// İster tekrar clientlara bilgi versin, isterse veritabanı işlemleri giib business işler üstlensin.
connection.invoke("SendMessage", "Kullanıcı Adı", "Mesaj")
    .catch(function (err) {
        return console.error(err.toString());
    });
```

## Haberleşme Nasıl Sağlanır?
.NET’te haberleşme, genellikle HTTP protokolü üzerinden RESTful API’ler veya WebSocket gibi teknolojiler kullanılarak sağlanır. SignalR gibi kütüphaneler, gerçek zamanlı haberleşme için WebSocket ve diğer uygun teknolojileri kullanır. Websocketler arka planda TCP dediğimiz haberleşme protokolünü kullanır. TCP, yani Transmission Control Protocol, internet üzerindeki bir iletişim metodudur ve bilgisayarlar arasındaki iletişimin küçük paketler halinde ve kayıpsız olarak gerçekleştirilmesini sağlayan bir protokoldür. TCP’nin en önemli özelliklerinden biri, veri gönderimi sırasında kimlik doğrulaması yapması ve verinin bütünlüğünü sağlamasıdır. Bu protokol, HTTP, HTTPS, POP3, SSH, SMTP, TELNET ve FTP gibi günlük hayatta sıkça kullanılan protokollerin veri iletiminde kullanılır ve UDP protokolüne göre daha yavaş ancak daha güvenli bir veri iletişimi sağlar.

TCP’nin çalışma mantığı üç ana aşamada incelenebilir:
* Bağlantı İsteği: İlk olarak hedefe bir bağlantı isteği gönderilir.
* Veri Transferi: Bağlantının gerçekleştiği onaylandıktan sonra veri transferi başlar.
* Bağlantının Sonlandırılması: Veri transferi tamamlandıktan sonra taraflara iletilerek bağlantı sonlandırılır.
TCP, internetin temel taşıdır ve güvenilir iletişim kurulmasını sağlar. Bu protokol sayesinde, internete bağlı cihazlar birbirleriyle etkili bir şekilde haberleşebilir.

## TypedSafe Nedir?
TypeSafe, .NET’te tür güvenliğini ifade eder. Tür güvenliği, bir değişkene yanlış türde bir değer atanmasını önleyen ve derleme zamanında tür doğrulaması yapan bir özelliktir.

## IHubContext Nedir?
IHubContext ASP.NET Core’da SignalR mimarisinin bir parçasıdır ve Hub sınıflarının dışında, farklı sınıflar veya controller’lar gibi yapılar üzerinden client ile server arasında etkileşime girmeyi sağlayan bir soyutlamadırIHubContext interface’i sayesinde, bir Hub sınıfı olmadan da SignalR özelliklerini kullanabilir ve mesaj gönderimi gibi işlemleri gerçekleştirebilirsiniz.

Örneğin, bir web uygulamasında, kullanıcıların gerçek zamanlı olarak birbirleriyle iletişim kurmasını sağlayan bir chat sistemi geliştiriyorsanız, IHubContext aracılığıyla, bir controller veya servis içerisinden kullanıcılara mesaj gönderebilirsiniz. Bu, özellikle arka planda çalışan işlemlerden kullanıcılara bildirim göndermek istediğinizde kullanışlıdır.

IHubContext’i kullanmak için, öncelikle bir Hub sınıfı tanımlamanız ve bu sınıfı uygulamanızın yapılandırma dosyasında (Startup.cs) servis olarak eklemeniz gerekmektedir. Ardından, IHubContext türünden bir nesneyi Dependency Injection aracılığıyla istediğiniz yere enjekte edebilir ve bu nesne üzerinden client’lara mesaj gönderimi yapabilirsiniz