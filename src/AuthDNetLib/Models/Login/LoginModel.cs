using AuthDNetLib.Helper.Messages;
using System.ComponentModel.DataAnnotations;

namespace AuthDNetLib.Models.Login;

public class LoginModel
{
    /// <summary>
    /// Nome de login do usuário para efetuar a autenticação.
    /// </summary>
    /// <remarks>
    /// A propriedade `Login` representa o nome de login único do usuário no sistema.
    /// Ele é usado para autenticação do usuário durante o login.
    /// </remarks>
    [Required(ErrorMessage = ErrorMessages.MsgLoginRequired)]
    public string Login { get; set; } = String.Empty;


    /// <summary>
    /// Senha do usuário.
    /// </summary>
    /// <remarks>
    /// A propriedade `Password` armazena a senha do usuário em um formato seguro utilizando o BCrypt para tratativas de segurança para o campo.
    /// A senha é usada para autenticar o usuário ao fazer login no sistema.
    /// Certifique-se de aplicar as melhores práticas de segurança para proteger esta propriedade.
    /// </remarks>
    [Required(ErrorMessage = ErrorMessages.MsgPasswordRequired)]
    public string Password { get; set; } = String.Empty;
}