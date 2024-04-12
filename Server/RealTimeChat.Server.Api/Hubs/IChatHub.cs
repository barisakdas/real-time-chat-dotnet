namespace RealTimeChat.Server.Api.Hubs;

public interface IChatHub
{
    public Task ReceiveMessage(string message);

    public Task ReceiveToGroupMessage(string message);

    public Task Clients(List<string> clients);

    public Task UserJoined(string connectionId);

    public Task UserLeaved(string connectionId);
}