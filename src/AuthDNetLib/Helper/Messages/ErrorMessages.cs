namespace AuthDNetLib.Helper.Messages;

/// <summary>
/// Fornece mensagens de erro padronizadas para validação de propriedades em classes do sistema de autenticação.
/// Esta classe contém constantes com mensagens descritivas utilizadas para validar propriedades, tais como nome, login, e-mail, senha, número de telefone, local de trabalho, status do usuário, data de registro, token de autenticação e etc.
/// Usando estas mensagens de erro padronizadas, é possível manter consistência na forma como erros de validação ou excessões lançadas são reportados em todo o sistema.
/// </summary>
public static class ErrorMessages
{
    /// <summary>
    /// Mensagem de erro indicando que o Id é obrigatório.
    /// </summary>
    public const string MsgIdRequired = "O id é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que o Id já está em uso.
    /// </summary>
    public const string MsgDuplicatedId = "O id já está em uso !";

    /// <summary>
    /// Mensagem de erro indicando que o nome é obrigatório.
    /// </summary>
    public const string MsgNameRequired = "O nome é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que o nome já está em uso.
    /// </summary>
    public const string MsgDuplicatedName = "O nome já está em uso !";

    /// <summary>
    /// Mensagem de erro indicando que o nome não pode conter mais de 60 caracteres.
    /// </summary>
    public const string MsgNameMaxLength = "O nome não pode conter mais de 60 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que o nome não pode conter menos de 6 caracteres.
    /// </summary>
    public const string MsgNameMinLength = "O nome não pode conter menos de 6 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que o login é obrigatório.
    /// </summary>
    public const string MsgLoginRequired = "O login é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que o login já está em uso.
    /// </summary>
    public const string MsgDuplicatedLogin = "O login já está em uso !";

    /// <summary>
    /// Mensagem de erro indicando que o login é inválido.
    /// </summary>
    public const string MsgInvalidLogin = "Login inválido !";

    /// <summary>
    /// Mensagem de erro indicando que o login não pode conter mais de 40 caracteres.
    /// </summary>
    public const string MsgLoginMaxLength = "O login não pode conter mais de 40 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que o login não pode conter menos de 6 caracteres.
    /// </summary>
    public const string MsgLoginMinLength = "O login não pode conter menos de 6 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que o e-mail é obrigatório.
    /// </summary>
    public const string MsgEmailRequired = "O e-mail é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que a confirmação do e-mail é obrigatória.
    /// </summary>
    public const string MsgConfirmEmailRequired = "Confirme seu email !";

    /// <summary>
    /// Mensagem de erro indicando que o e-mail já está em uso.
    /// </summary>
    public const string MsgDuplicatedEmail = "O e-mail já está em uso !";

    /// <summary>
    /// Mensagem de erro indicando que os e-mails não conferem.
    /// </summary>
    public const string MsgEmailsUnmatching = "Os e-mails não conferem !";

    /// <summary>
    /// Mensagem de erro indicando que o e-mail não pode conter mais de 100 caracteres.
    /// </summary>
    public const string MsgEmailMaxLength = "O e-mail não pode conter mais de 100 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que o formato do e-mail inserido é inválido.
    /// </summary>
    public const string MsgEmailInvalid = "O formato do e-mail inserido é inválido !";

    /// <summary>
    /// Mensagem de erro indicando que a senha é obrigatória.
    /// </summary>
    public const string MsgPasswordRequired = "A senha é obrigatória !";

    /// <summary>
    /// Mensagem de erro indicando que a confirmação da senha é obrigatória.
    /// </summary>
    public const string MsgConfirmPasswordRequired = "Confirme sua senha !";

    /// <summary>
    /// Mensagem de erro indicando que as senhas não conferem.
    /// </summary>
    public const string MsgPasswordsUnmatching = "As senhas não conferem !";

    /// <summary>
    /// Mensagem de erro indicando que a senha é inválida.
    /// </summary>
    public const string MsgInvalidPassword = "Senha inválida !";

    /// <summary>
    /// Mensagem de erro indicando que a senha não atende aos critérios de segurança.
    /// </summary>
    public const string MsgPasswordNotStrong = "A senha não atende aos critérios de segurança !";

    /// <summary>
    /// Mensagem de erro indicando que a senha deve ter pelo menos 8 caracteres.
    /// </summary>
    public const string MsgPasswordMinLength = "A senha deve ter pelo menos 8 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que a senha deve conter no mínimo 8 caracteres, letras maiúsculas, minúsculas, números e símbolos.
    /// </summary>
    public const string MsgPasswordRegex = "A senha deve conter no mínimo 8 caracteres, letras maiúsculas, minúsculas, números e símbolos !";

    /// <summary>
    /// Mensagem de erro indicando que o telefone é obrigatório.
    /// </summary>
    public const string MsgPhoneRequired = "O telefone é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que o telefone já está em uso.
    /// </summary>
    public const string MsgDuplicatedPhone = "O telefone já está em uso !";

    /// <summary>
    /// Mensagem de erro indicando que o telefone não pode conter mais de 15 caracteres.
    /// </summary>
    public const string MsgPhoneMaxLength = "O telefone não pode conter mais de 15 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que o número de telefone é inválido.
    /// </summary>
    public const string MsgPhoneInvalid = "O número de telefone é inválido. Utilize o formato: (XX) XXXXX-XXXX";

    /// <summary>
    /// Mensagem de erro indicando que o local de trabalho é obrigatório.
    /// </summary>
    public const string MsgWorkplaceRequired = "O local de trabalho é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que o local de trabalho não pode conter mais de 40 caracteres.
    /// </summary>
    public const string MsgWorkplaceMaxLength = "O local de trabalho não pode conter mais de 40 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que o local de trabalho não pode conter menos de 4 caracteres.
    /// </summary>
    public const string MsgWorkplaceMinLength = "O local de trabalho não pode conter menos de 4 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que o status do usuário é obrigatório.
    /// </summary>
    public const string MsgUserStatsRequired = "O status do usuário é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que a data de registro é obrigatória.
    /// </summary>
    public const string MsgRegisterDateRequired = "Informe uma data de registro !";

    /// <summary>
    /// Mensagem de erro indicando que o nível de usuário é obrigatório.
    /// </summary>
    public const string MsgRoleRequired = "O nível de usuário é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que o token de autenticação é obrigatório.
    /// </summary>
    public const string MsgSessionTokenRequired = "O token de autenticação é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que o Id do token de autenticação é obrigatório.
    /// </summary>
    public const string MsgTokenIdRequired = "O Id do token de autenticação é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que a data de registro do token de autenticação é obrigatória.
    /// </summary>
    public const string MsgTokenRegisterDateRequired = "A data de registro do token de autenticação é obrigatória !";

    /// <summary>
    /// Mensagem de erro indicando que o relacionamento de usuário com o token de autenticação é obrigatório.
    /// </summary>
    public const string MsgTokenUserIdRequired = "O relacionamento de usuário com o token de autenticação é obrigatório !";

    /// <summary>
    /// Mensagem de erro indicando que ocorreu uma falha ao gerar um token duplicado.
    /// </summary>
    public const string MsgDuplicateSessionToken = "Falha por gerar token duplicado !";

    /// <summary>
    /// Mensagem de erro indicando que o token não pode conter mais de 150 caracteres.
    /// </summary>
    public const string MsgTokenMaxLength = "O token não pode conter mais de 150 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que o token não pode conter menos de 10 caracteres.
    /// </summary>
    public const string MsgTokenMinLength = "O token não pode conter menos de 10 caracteres !";

    /// <summary>
    /// Mensagem de erro indicando que o HttpContext não pode ser nulo.
    /// </summary>
    public const string MsgNullHttpContext = "HttpContext não pode ser nulo.";

    /// <summary>
    /// Mensagem de erro indicando que não foi possível desserializar os dados passados como parâmetro.
    /// </summary>
    public const string MsgJSONDesserializeError = "Não foi possível desserializar os dados passados como parâmetro.";

    /// <summary>
    /// Mensagem de erro indicando que não foi possível serializar os dados passados como parâmetro.
    /// </summary>
    public const string MsgJSONSerializeError = "Não foi possível serializar os dados passados como parâmetro.";

    /// <summary>
    /// Mensagem de erro indicando que o navegador não é compatível com JavaScript ou o JavaScript está desativado.
    /// </summary>
    public const string MsgJSDisable = "O navegador que você está usando não é compatível com JavaScript, ou o JavaScript está desativado.Para manter seus dados seguros, utilize um navegador que tenha o JavaScript ativado.";

    /// <summary>
    /// Mensagem de erro indicando que os dados de acesso informados são inválidos.
    /// </summary>
    public const string MsgUnauthorizedAccess = "Os dados de acesso informados são inválidas !";

    /// <summary>
    /// Mensagem de erro indicando que a conta foi bloqueada por excesso de tentativas.
    /// </summary>
    public const string MsgLockedAccount = "Conta bloqueada por excesso de tentativas! Tente novamente mais tarde.";

    /// <summary>
    /// Mensagem de erro indicando que nenhuma sessão ativa foi encontrada para ser encerrada.
    /// </summary>
    public const string MsgActiveSessionNotFound = "Nenhuma sessão ativa encontrada para ser encerrada.";

    /// <summary>
    /// Mensagem de erro indicando que o token JWT gerado não é válido.
    /// </summary>
    public const string MsgJWTTokenInvalid = "O token JWT gerado não é válido.";

    /// <summary>
    /// Mensagem de erro indicando que a chave secreta JWT não está configurada.
    /// </summary>
    public const string MsgJWTSecureKeyNotConfigured = "A chave secreta JWT não está configurada.";

    /// <summary>
    /// Mensagem de erro indicando que o tipo fornecido não possui uma propriedade 'Login'.
    /// </summary>
    public const string MsgLoginNotExist = "O Tipo fornecido não possui uma propriedade 'Login'.";

    /// <summary>
    /// Mensagem de erro indicando que o valor da propriedade 'Login' não pode ser nulo.
    /// </summary>
    public const string MsgLoginIsNull = "O valor da propriedade 'Login' não pode ser nulo.";

    /// <summary>
    /// Mensagem de erro indicando que o tipo fornecido não possui uma propriedade 'Password'.
    /// </summary>
    public const string MsgPasswordNotExist = "O Tipo fornecido não possui uma propriedade 'Password'.";
}