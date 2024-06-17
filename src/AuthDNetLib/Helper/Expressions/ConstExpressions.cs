namespace AuthDNetLib.Helper.Expressions;

/// <summary>
/// Classe que define constantes para expressões regulares e formatos utilizados em partes do sistema.
/// </summary>
public static class ConstExpressions
{
    /// <summary>
    /// Expressão regular para validar senhas. A senha deve conter no mínimo 8 caracteres, pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial.
    /// </summary>
    public const string StrongPasswordRegex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{8,}$";

    /// <summary>
    /// Expressão regular para validar números de telefone no formato (XX) XXXXX-XXXX.
    /// </summary>
    public const string PhoneNumberRegex = @"^\(?(?:[0-9]{2})\)?[-. ]?(?:[2-9]|9[1-9])[0-9]{3}[-. ]?[0-9]{4}$";

    /// <summary>
    /// Expressão regular para validar E-mails seguindo o padrão RFC 5322.
    /// </summary>
    public const string EmailRegex = @"^(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])"")@(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-zA-Z0-9-]*[a-zA-Z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)])$";

    /// <summary>
    /// Formato de data utilizado para exibição no formato ano-mês-dia hora:minuto:segundo.
    /// </summary>
    public const string DateTimeFormat = "{0:yyyy-MM-dd HH:mm:ss}";
}