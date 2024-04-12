using Microsoft.AspNetCore.Mvc;

namespace RealTimeChat.Server.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HubController(IHubContext<ChatHub> hubContext) : ControllerBase
{
    // Hub içerisinden ulaştığımız özellikler ve metotların başka bir servis veya yapı içerisinden erişebilmemiz için IHubContext arayüzünü kullanıyoruz.

    [HttpGet]
    public async Task<IActionResult> SendMessageToClient(string message)
    {
        await hubContext.Clients.All.SendAsync(message);

        return Ok();
    }
}