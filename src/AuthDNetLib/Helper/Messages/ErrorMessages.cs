namespace AuthDNetLib.Helper.Messages;

/// <summary>
/// Fornece mensagens de erro padronizadas para validação de propriedades em classes relacionadas a autenticação.
/// Esta classe contém constantes de mensagens de erro que são usadas para validar propriedades de classes de usuários,
/// tais como atributos como nome, login, e-mail, senha, número de telefone, local de trabalho, status do usuário, data de registro e token de autenticação.
/// Usando estas mensagens de erro padronizadas, é possível manter consistência na forma como erros de validação são reportados em todo o sistema.
/// </summary>
public static class ErrorMessages
{
    // Mensagens de Erro para o Id
    public const string MsgIdRequired = "O id é obrigatório !";
    public const string MsgDuplicatedId = "O id já está em uso !";

    // Mensagens de Erro para o Nome
    public const string MsgNameRequired = "O nome é obrigatório !";
    public const string MsgDuplicatedName = "O nome já está em uso !";
    public const string MsgNameMaxLength = "O nome não pode conter mais de 60 caracteres !";
    public const string MsgNameMinLength = "O nome não pode conter menos de 6 caracteres !";

    // Mensagens de Erro para o Login
    public const string MsgLoginRequired = "O login é obrigatório !";
    public const string MsgDuplicatedLogin = "O login já está em uso !";
    public const string MsgInvalidLogin = "Login inválido !";
    public const string MsgLoginMaxLength = "O login não pode conter mais de 40 caracteres !";
    public const string MsgLoginMinLength = "O login não pode conter menos de 6 caracteres !";

    // Mensagens de Erro para o Email
    public const string MsgEmailRequired = "O e-mail é obrigatório !";
    public const string MsgConfirmEmailRequired = "Confirme seu email !";
    public const string MsgDuplicatedEmail = "O e-mail já está em uso !";
    public const string MsgEmailsUnmatching = "Os e-mails não conferem !";
    public const string MsgEmailMaxLength = "O e-mail não pode conter mais de 100 caracteres !";
    public const string MsgEmailInvalid = "O formato do e-mail inserido é inválido !";

    // Mensagens de Erro para a Senha
    public const string MsgPasswordRequired = "A senha é obrigatória !";
    public const string MsgConfirmPasswordRequired = "Confirme sua senha !";
    public const string MsgPasswordsUnmatching = "As senhas não conferem !";
    public const string MsgInvalidPassword = "Senha inválida !";
    public const string MsgPasswordNotStrong = "A senha não atende aos critérios de segurança !";
    public const string MsgPasswordMinLength = "A senha deve ter pelo menos 8 caracteres !";
    public const string MsgPasswordRegex = "A senha deve conter no mínimo 8 caractéres, letras maiúsculas, minúsculas, números e símbolos !";

    // Mensagens de Erro para o Telefone
    public const string MsgPhoneRequired = "O telefone é obrigatório !";
    public const string MsgDuplicatedPhone = "O telefone já está em uso !";
    public const string MsgPhoneMaxLength = "O telefone não pode conter mais de 15 caracteres !";
    public const string MsgPhoneInvalid = "O número de telefone é inválido. Utilize o formato: (XX) XXXXX-XXXX";

    // Mensagens de Erro para o Local de Trabalho
    public const string MsgWorkplaceRequired = "O local de trabalho é obrigatório !";
    public const string MsgWorkplaceMaxLength = "O local de trabalho não pode conter mais de 40 caracteres !";
    public const string MsgWorkplaceMinLength = "O local de trabalho não pode conter menos de 4 caracteres !";

    // Mensagens de Erro para o Status do Usuário
    public const string MsgUserStatsRequired = "O status do usuário é obrigatório !";

    // Mensagens de Erro para a Data de Registro
    public const string MsgRegisterDateRequired = "Informe uma data de registro !";

    // Mensagens de Erro para Role
    public const string MsgRoleRequired = "O nível de usuário é obrigatório !";

    // Mensagens de Erro para o Token de Autenticação
    public const string MsgSessionTokenRequired = "O token de autenticação é obrigatório !";
    public const string MsgTokenIdRequired = "O Id do token de autenticação é obrigatório !";
    public const string MsgTokenRegisterDateRequired = "A data de registro do token de autenticação é obrigatória !";
    public const string MsgTokenUserIdRequired = "O relacionamento de usuário com o token de autenticação é obrigatório !";
    public const string MsgDuplicateSessionToken = "Falha por gerar token duplicado !";
    public const string MsgTokenMaxLength = "O token não pode conter mais de 150 caracteres !";
    public const string MsgTokenMinLength = "O token não pode conter menos de 10 caracteres !";

    // Erro em HttpContext
    public const string MsgNullHttpContext = "HttpContext não pode ser nulo.";

    // Erro em Serializar e desserializar objetos
    public const string MsgJSONDesserializeError = "Não foi possível desserializar os dados passados como parâmetro.";
    public const string MsgJSONSerializeError = "Não foi possível serializar os dados passados como parâmetro.";

    // JS desabilitado no navegador
    public const string MsgJSDisable = "O navegador que você está usando não é compatível com JavaScript, ou o JavaScript está desativado.Para manter seus dados seguros, utilize um navegador que tenha o JavaScript ativado.";

    // Acesso não autorizado
    public const string MsgUnauthorizedAccess = "Os dados de acesso informados são inválidas !";
    public const string MsgLockedAccount = "Conta bloqueada por excesso de tentativas ! Tente novamente mais tarde.";

    // Erros de sessão
    public const string MsgActiveSessionNotFound = "Nenhuma sessão ativa encontrada para ser encerrada.";

    // Erros com chaves secretas
    public const string MsgJWTTokenInvalid = "O token JWT gerado não é válido.";
    public const string MsgJWTSecureKeyNotConfigured = "A chave secreta JWT não está configurada.";

    // Erros com Relexão de propriedades
    public const string MsgLoginNotExist = "O Tipo fornecido não possui uma propriedade 'Login'.";
    public const string MsgLoginIsNull = "O valor da propriedade 'Login' não pode ser nulo.";
    public const string MsgPasswordNotExist = "O Tipo fornecido não possui uma propriedade 'Password'.";

}