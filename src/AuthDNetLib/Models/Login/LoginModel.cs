using AuthDNetLib.Helper.Messages;
using System.ComponentModel.DataAnnotations;

namespace AuthDNetLib.Models.Login;

public class LoginModel
{
    [Required(ErrorMessage = ErrorMessages.MsgLoginRequired)]
    public string LoginName { get; set; } = String.Empty;

    [Required(ErrorMessage = ErrorMessages.MsgPasswordRequired)]
    public string Password { get; set; } = String.Empty;
}