using System.Text;
using AuthDNetLib.Helper.Messages;
using AuthDNetLib.Helper.Transfer.Data;
using AuthDNetLib.Keys.Session;
using AuthDNetLib.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthDNetRCL.Areas.Auth.Views.ViewComponents;

public class Navbar(ILogger<Navbar> logger, IHttpContextAccessor httpContextAccessor) : ViewComponent
{
    private readonly ILogger<Navbar> _logger = logger ?? throw new InvalidOperationException(nameof(logger));

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new InvalidOperationException(nameof(httpContextAccessor));

    public async Task<IViewComponentResult> InvokeAsync()
    {
        string? userSession = _httpContextAccessor.HttpContext?.Session.GetString(SessionString.SessionConnectString);

        if (string.IsNullOrEmpty(userSession))
            return View();

        try
        {
            var users = await JSONDataTransfer<TUser>.JSONSecureDataDesserialize(userSession) ?? throw new InvalidOperationException(ErrorMessages.MsgJSONDesserializeError);

            return View(users);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("{Date} - {exceptionMessage} : {Message}", DateTime.UtcNow, ex.Message, LogMessages.MsgErrorNavbarIOE);
            return View();
        }
        catch(Exception ex)
        {
            _logger.LogError("{Date} - {exceptionMessage}: {Message}", DateTime.UtcNow, ex.Message, LogMessages.MsgErrorNavbarGeenric);
            return View();
        }
    }
}