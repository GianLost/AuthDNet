using System.Text;
using AuthDNetLib.Helper.Transfer.Data;
using AuthDNetLib.Keys.Session;
using AuthDNetLib.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthDNetRCL.Areas.Auth.Views.ViewComponents;

public class Navbar(IHttpContextAccessor httpContextAccessor) : ViewComponent
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new InvalidOperationException(nameof(httpContextAccessor));

    public async Task<IViewComponentResult> InvokeAsync()
    {
        string? userSession = _httpContextAccessor?.HttpContext?.Session.GetString(SessionString.SessionConnectString);

        if (string.IsNullOrEmpty(userSession))
            return View();

        TUser? users = await JSONDataTransfer<TUser>.JSONSecureDataDesserialize(userSession) ?? throw new InvalidOperationException("Falha ao desserializar os dados criptografados do usuário.");

        return View(users);
    }
}