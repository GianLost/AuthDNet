using AuthDNetLib.Helper.Expressions;
using AuthDNetLib.Helper.Messages;
using AuthDNetLib.Models.Tokens;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthDNetLib.Models.Users;

/// <summary>
/// Classe base abstrata para usuários que define o uso de uma chave primária única para identificação de´tipos de usuários no sistema.
/// Herda as propriedades e métodos comuns da classe base <see cref="TUser{TKey}"/>.
/// </summary>
public class TUser : TUser<string>
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="TUser"/> com um novo identificador único.
    /// </summary>
    /// <remarks>
    /// Cria uma nova instância da classe `TUser` e gera um novo identificador único para o usuário atrvés do método <see cref="Guid.NewGuid()" />.
    /// Isso ajuda a garantir que cada usuário tenha um identificador exclusivo.
    /// </remarks>
    public TUser()
    {
        Initialize();
    }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="TUser"/> com um nome de login específico.
    /// </summary>
    /// <param name="_login">Nome de login do usuário.</param>
    /// <remarks>
    /// Cria uma nova instância da classe `TUser` com o nome de login fornecido.
    /// O nome de login é armazenado na propriedade `Login` e pode ser usado para autenticação do usuário.
    /// </remarks>
    public TUser(string _login)
    {
        Initialize();
    }

    /// <summary>
    /// Número de tentativas de login falhas.
    /// </summary>
    /// <remarks>
    /// A propriedade `FailedAttempts` mantém o número de tentativas de login falhas para este usuário.
    /// </remarks>
    public int FailedAttempts { get; set; }

    /// <summary>
    /// Indica se o usuário está bloqueado devido a múltiplas tentativas de login falhas.
    /// </summary>
    /// <remarks>
    /// A propriedade `IsLockedOut` indica se o usuário está bloqueado devido a múltiplas tentativas de login falhas.
    /// </remarks>
    public bool IsLockedOut { get; set; }

    /// <summary>
    /// Data e hora em que o bloqueio do usuário expirará, permitindo nova tentativa de login.
    /// </summary>
    /// <remarks>
    /// A propriedade `LockoutEnd` indica a data e hora em que o bloqueio do usuário expirará.
    /// Após o tempo de bloqueio expirar, o usuário poderá tentar fazer login novamente.
    /// </remarks>
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = ConstExpressions.DateTimeFormat, ApplyFormatInEditMode = true)]
    public DateTime? LockoutEnd { get; set; }

    /// <summary>
    /// Data e hora da última tentativa de login falha.
    /// </summary>
    /// <remarks>
    /// A propriedade `LastFailedAttempt` armazena a data e hora da última tentativa de login falha do usuário.
    /// É utilizada para rastrear quando a última tentativa de login falha ocorreu, permitindo implementar lógica como
    /// o bloqueio temporário após várias tentativas falhas.
    /// </remarks>
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = ConstExpressions.DateTimeFormat, ApplyFormatInEditMode = true)]
    public virtual DateTime? LastFailedAttempt { get; set; }

    private void Initialize()
    {
        Id = Guid.NewGuid().ToString();
        FailedAttempts = 0;
        IsLockedOut = false;
    }
}

/// <summary>
/// Classe base para usuários no sistema, com propriedades e métodos comuns para diferentes tipos de usuários.
/// </summary>
/// <typeparam name="TKey">O tipo de chave primária para identificar o usuário de forma única.</typeparam>
public abstract class TUser<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="TUser{TKey}"/>.
    /// </summary>
    public TUser() { }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="TUser{TKey}"/> com o nome de login fornecido.
    /// </summary>
    /// <param name="_login">Nome de login do usuário.</param>
    public TUser(string _login) : this()
    {
        Login = _login;
    }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="TUser{TKey}"/> com os atributos fornecidos.
    /// </summary>
    /// <param name="_id">Chave primária que identifica o usuário de forma única.</param>
    /// <param name="_name">Nome completo do usuário.</param>
    /// <param name="_login">Nome de login do usuário.</param>
    /// <param name="_email">Endereço de e-mail do usuário.</param>
    /// <param name="_confirmEmail">Confirmação do endereço de e-mail do usuário.</param>
    /// <param name="_password">Senha do usuário, armazenada de forma segura.</param>
    /// <param name="_confirmPassword">Confirmação da senha do usuário.</param>
    /// <param name="_cellPhone">Número de telefone celular do usuário.</param>
    /// <param name="_workplace">Local de trabalho do usuário.</param>
    /// <param name="_registerDate">Data de registro do usuário.</param>
    /// <param name="_modifyDate">Data da última modificação das informações do usuário.</param>
    /// <param name="_sessionToken">Token de autenticação associado ao usuário.</param>
    /// <param name="_tokens">Representam uma coleção de tokens do usuário.</param>
    protected TUser(TKey _id, string _name, string _login, string _email, string _confirmEmail, string _password, string _confirmPassword, string _cellPhone, string _workplace, DateTime? _registerDate, DateTime? _modifyDate, string _sessionToken, ICollection<Token> _tokens) : this()
    {
        Id = _id;
        Name = _name;
        Login = _login;
        Email = _email;
        ConfirmEmail = _confirmEmail;
        Password = _password;
        ConfirmPassword = _confirmPassword;
        CellPhone = _cellPhone;
        Workplace = _workplace;
        RegisterDate = _registerDate ?? DateTime.UtcNow;
        ModifyDate = _modifyDate;
        AuthToken = _sessionToken;
        Tokens = _tokens ?? [];
    }

    /// <summary>
    /// Chave primária que identifica de forma exclusiva cada usuário no sistema.
    /// </summary>
    [Key]
    [Required(ErrorMessage = ErrorMessages.MsgIdRequired)]
    [PersonalData]
    public virtual TKey? Id { get; set; }

    /// <summary>
    /// Nome completo do usuário.
    /// </summary>
    /// <remarks>
    /// A propriedade `Name` armazena o nome completo do usuário.
    /// Pode ser usado para exibir o nome do usuário em interfaces de usuário e para fins de identificação.
    /// </remarks>
    [Required(ErrorMessage = ErrorMessages.MsgNameRequired)]
    [StringLength(60, ErrorMessage = ErrorMessages.MsgNameMaxLength)]
    [MinLength(6, ErrorMessage = ErrorMessages.MsgNameMinLength)]
    [ProtectedPersonalData]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Nome de login do usuário.
    /// </summary>
    /// <remarks>
    /// A propriedade `Login` representa o nome de login único do usuário no sistema.
    /// Ele é usado para autenticação do usuário durante o login.
    /// </remarks>
    [Required(ErrorMessage = ErrorMessages.MsgLoginRequired)]
    [StringLength(40, ErrorMessage = ErrorMessages.MsgLoginMaxLength)]
    [MinLength(6, ErrorMessage = ErrorMessages.MsgLoginMinLength)]
    [ProtectedPersonalData]
    public virtual string? Login { get; set; }

    /// <summary>
    /// Endereço de e-mail do usuário.
    /// </summary>
    /// <remarks>
    /// A propriedade `Email` armazena o endereço de e-mail do usuário, que pode ser usado para comunicação ou recuperação de senha.
    /// </remarks>
    [Required(ErrorMessage = ErrorMessages.MsgEmailRequired)]
    [StringLength(100, ErrorMessage = ErrorMessages.MsgEmailMaxLength)]
    [EmailAddress(ErrorMessage = ErrorMessages.MsgEmailInvalid)]
    [ProtectedPersonalData]
    public virtual string? Email { get; set; }

    /// <summary>
    /// Confirmar endereço de e-mail do usuário.
    /// </summary>
    /// <remarks>
    /// Este campo é usado para confirmar o endereço de e-mail fornecido pelo usuário durante o registro.
    /// </remarks>
    [NotMapped]
    [Required(ErrorMessage = ErrorMessages.MsgConfirmEmailRequired)]
    [StringLength(100, ErrorMessage = ErrorMessages.MsgEmailMaxLength)]
    [EmailAddress(ErrorMessage = ErrorMessages.MsgEmailInvalid)]
    [ProtectedPersonalData]
    public virtual string? ConfirmEmail { get; set; }

    /// <summary>
    /// Senha do usuário.
    /// </summary>
    /// <remarks>
    /// A propriedade `Password` armazena a senha do usuário em um formato seguro utilizando o BCrypt para tratativas de segurança para o campo.
    /// A senha é usada para autenticar o usuário ao fazer login no sistema.
    /// Certifique-se de aplicar as melhores práticas de segurança para proteger esta propriedade.
    /// </remarks>
    [Required(ErrorMessage = ErrorMessages.MsgPasswordRequired)]
    [MinLength(8, ErrorMessage = ErrorMessages.MsgPasswordMinLength)]
    [RegularExpression(ConstExpressions.StrongPasswordRegex, ErrorMessage = ErrorMessages.MsgPasswordRegex)]
    [StringLength(150)]
    [ProtectedPersonalData]
    public virtual string? Password { get; set; }

    /// <summary>
    /// Confirmar senha do usuário.
    /// </summary>
    /// <remarks>
    /// Este campo é usado para confirmar a senha fornecida pelo usuário durante o registro ou alteração de senha.
    /// </remarks>

    [NotMapped]
    [Required(ErrorMessage = ErrorMessages.MsgConfirmPasswordRequired)]
    [MinLength(8, ErrorMessage = ErrorMessages.MsgPasswordMinLength)]
    [RegularExpression(ConstExpressions.StrongPasswordRegex, ErrorMessage = ErrorMessages.MsgPasswordRegex)]
    [StringLength(150)]
    [ProtectedPersonalData]
    public virtual string? ConfirmPassword { get; set; }

    /// <summary>
    /// Número de telefone do usuário.
    /// </summary>
    /// <remarks>
    /// A propriedade `CellPhone` armazena o número de telefone celular do usuário.
    /// Pode ser usado para verificação de identidade ou para comunicação direta com o usuário.
    /// </remarks>
    [Required(ErrorMessage = ErrorMessages.MsgPhoneRequired)]
    [StringLength(15, ErrorMessage = ErrorMessages.MsgPhoneMaxLength)]
    [RegularExpression(ConstExpressions.PhoneNumberRegex, ErrorMessage = ErrorMessages.MsgPhoneInvalid)]
    public virtual string? CellPhone { get; set; }

    /// <summary>
    /// Local de trabalho do usuário.
    /// </summary>
    /// <remarks>
    /// A propriedade `Workplace` armazena informações sobre o local de trabalho do usuário.
    /// Pode ser útil para personalizar a experiência do usuário com base em sua organização ou função.
    /// </remarks>
    [Required(ErrorMessage = ErrorMessages.MsgWorkplaceRequired)]
    [StringLength(40, ErrorMessage = ErrorMessages.MsgWorkplaceMaxLength)]
    [MinLength(4, ErrorMessage = ErrorMessages.MsgWorkplaceMinLength)]
    public virtual string? Workplace { get; set; }

    /// <summary>
    /// Data de registro do usuário.
    /// </summary>
    /// <remarks>
    /// A propriedade `RegisterDate` armazena a data em que o usuário foi registrado no sistema.
    /// Pode ser usado para rastrear a duração da conta do usuário no sistema.
    /// </remarks>
    [Required(ErrorMessage = ErrorMessages.MsgRegisterDateRequired)]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = ConstExpressions.DateTimeFormat, ApplyFormatInEditMode = true)]
    public virtual DateTime RegisterDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data da última modificação dos dados do usuário.
    /// </summary>
    /// <remarks>
    /// A propriedade `ModifyDate` armazena a data da última modificação dos dados do usuário.
    /// Pode ser útil para rastrear mudanças nos dados do usuário ao longo do tempo.
    /// </remarks>
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = ConstExpressions.DateTimeFormat, ApplyFormatInEditMode = true)]
    public virtual DateTime? ModifyDate { get; set; }

    /// <summary>
    /// Token de autenticação do usuário.
    /// </summary>
    /// <remarks>
    /// A propriedade `AuthToken` armazena um token de autenticação gerado de forma única e aleatória associado ao usuário.
    /// Pode ser usado para autenticar o usuário em sessões subsequentes.
    /// </remarks>
    [StringLength(150, ErrorMessage = ErrorMessages.MsgTokenMaxLength)]
    [ProtectedPersonalData]
    public virtual string? AuthToken { get; set; }

    /// <summary>
    /// Coleção de tokens associados ao usuário.
    /// </summary>
    [ProtectedPersonalData]
    public virtual ICollection<Token> Tokens { get; set; } = [];
}
