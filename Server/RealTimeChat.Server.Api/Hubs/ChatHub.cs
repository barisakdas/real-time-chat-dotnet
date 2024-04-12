namespace RealTimeChat.Server.Api.Hubs;

/// <summary>
/// ChatHub, SignalR altyapısını kullanarak gerçek zamanlı mesajlaşma işlevselliği sağlayan bir hub sınıfıdır.
/// İstemcilerin sunucuya bağlanmasını, mesaj göndermesini ve almasını sağlar.
/// Ayrıca, istemcileri gruplara ekleyip çıkarmak ve gelen mesajları işleyip geri göndermek gibi işlemleri destekler.
/// </summary>
public class ChatHub : Hub<IChatHub>
{
    // Mevcutta bağlı olan clientların idlerinin listesi.
    // Bir client bağlandığında ids listeye eklenecek, ayrıldığında çıkarılacak.
    public List<string> ClientList = new List<string>();

    /// <summary>Tüm bağlı istemcilere mesaj gönderir.</summary>
    /// <param name="message">Gönderilecek mesaj.</param>
    public async Task SendMessageToAll(string message)
        => await Clients.All.ReceiveMessage(message);

    /// <summary>Sadece istek yapan istemciye mesaj gönderir.</summary>
    /// <param name="message">Gönderilecek mesaj.</param>
    public async Task SendMessageToCaller(string message)
        => await Clients.Caller.ReceiveMessage(message);

    /// <summary>Sadece istek yapan istemciye mesaj gönderir.</summary>
    /// <param name="message">Gönderilecek mesaj.</param>
    public async Task SendMessageToOther(string message)
        => await Clients.Others.ReceiveMessage(message);

    /// <summary>Belirli bir gruptaki istemcilere mesaj gönderir.</summary>
    /// <param name="groupName">Mesajın gönderileceği grup adı.</param>
    /// <param name="message">Gönderilecek mesaj.</param>
    public async Task SendMessageToGroup(string groupName, string message)
        => await Clients.Group(groupName).ReceiveToGroupMessage(message);

    /// <summary>Belirli bir istemciye mesaj gönderir.</summary>
    /// <param name="connectionId">Mesajın gönderileceği istemcinin bağlantı kimliği.</param>
    /// <param name="message">Gönderilecek mesaj.</param>
    public async Task SendMessageToClient(string connectionId, string message)
        => await Clients.Client(connectionId).ReceiveToGroupMessage(message);

    /// <summary>Bir istemciyi belirli bir gruba ekler.</summary>
    /// <param name="groupName">Eklenecek grup adı.</param>
    public async Task AddToGroup(string groupName)
    {
        // İlgili client gruba ekleniyor.
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        // Gruba eklendiğinin bilgisi kendisine veriliyor.
        await Clients.Caller.ReceiveToGroupMessage($"{groupName} isimli gruba eklendiniz.");

        // Gruptaki diğer paydaşlara gruba eklendiğini gösteriyoruz.
        await Clients.Group(groupName).ReceiveToGroupMessage($"{Context.ConnectionId} id li kullanıcı gruba eklendi.");
    }

    /// <summary>Bir istemciyi belirli bir gruptan çıkarır.</summary>
    /// <param name="groupName">Çıkarılacak grup adı.</param>
    public async Task RemoveFromGroup(string groupName)
    {
        // İlgili client gruptan çıkartılıyor.
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        // Gruptan ayrıldığının bilgisi kendisine veriliyor.
        await Clients.Caller.ReceiveToGroupMessage($"{groupName} isimli gruptan ayrıldınız.");

        // Gruptan ayrıldığının bilgisi gruptaki diğer paydaşlara veriliyor.
        await Clients.Group(groupName).ReceiveToGroupMessage($"{Context.ConnectionId} id li kullanıcı gruptan ayrıldı.");
    }

    /// <summary>Herhangi bir client bağlantı kurduğunda çalışacak olan metot.</summary>
    public override async Task OnConnectedAsync()
    {
        // Bir clientın huba bağlanması önemli bir event arz edebilir.
        // Örneğin online oyunlarda `XXXX odaya katıldı.`
        // Bu tip bir durumda diğer kullanıcıların bilgilendirilmesi gerekirse bu metot içerisindeki kodlar çalışacaktır.

        // ConnectionId: Huba bağlantı gerçekleştiren sistemdeki clientları bir birinden ayırmamızı sağlayan, sistem tarafından üretilen bir string değerdir.
        await Clients.Others.UserJoined($"{Context.ConnectionId} id li client bağlantı sağladı.");

        // Client bağlandığı için listeye ekliyoruz.
        ClientList.Add(Context.ConnectionId);

        // Bu yeni durumda oluşan client idlerinin listesini client tarafına gönderebiliriz.
        await Clients.Others.Clients(ClientList);
    }

    /// <summary>Herhangi bir client bağlantı kopardığında çalışacak olan metot.</summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Bir clientın hubtan bağlantısını koparması önemli bir event arz edebilir.
        // Örneğin online oyunlarda `XXXX odadan ayrıldı.`
        // Bu tip bir durumda diğer kullanıcıların bilgilendirilmesi gerekirse bu metot içerisindeki kodlar çalışacaktır.

        // ConnectionId: Huba bağlantı gerçekleştiren sistemdeki clientları bir birinden ayırmamızı sağlayan, sistem tarafından üretilen bir string değerdir.
        await Clients.Others.UserLeaved($"{Context.ConnectionId} id li client bağlantı kopardı.");

        // Client ayrıldığı için listeden çıkartıyoruz.
        ClientList.Remove(Context.ConnectionId);

        // Bu yeni durumda oluşan client idlerinin listesini client tarafına gönderebiliriz.
        await Clients.Others.Clients(ClientList);
    }

    /// <summary>Asenkron olarak bir client'ı belirli bir gruba ekler.</summary>
    /// <param name="connectionId">Gruba eklenecek client'ın bağlantı kimliği.</param>
    /// <param name="groupName">Client'ın ekleneceği grup adı.</param>
    /// <returns>Task, işlemin asenkron olarak tamamlanmasını temsil eder.</returns>
    public async Task AddToGroupAsync(string connectionId, string groupName)
        => Groups.AddToGroupAsync(connectionId, groupName);
}