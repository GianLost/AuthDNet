using AuthDNetLib.Helper.Expressions;
using AuthDNetLib.Helper.Messages;
using AuthDNetLib.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthDNetLib.Models.Tokens;

public class Token : Token<string, TUser>
{
    public Token()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Token(string _sessionToken)
    {
        SessionToken = _sessionToken;
    }
}

public abstract class Token<TKey, T> where TKey : IEquatable<TKey> where T : class
{
    public Token() { }

    public Token(string _sessionToken) : this()
    {
        SessionToken = _sessionToken;
    }

    protected Token(TKey _id, string _sessionToken, DateTime? _registerDate, TKey _userId)
    {
        Id = _id;
        SessionToken = _sessionToken;
        RegisterDate = _registerDate ?? DateTime.UtcNow;
        UserId = _userId;
    }

    [Key, Required(ErrorMessage = ErrorMessages.MsgTokenIdRequired)]
    public TKey? Id { get; set; }

    [Required(ErrorMessage = ErrorMessages.MsgSessionTokenRequired)]
    [StringLength(150), MaxLength(150, ErrorMessage = ErrorMessages.MsgTokenMaxLength)]
    [MinLength(10, ErrorMessage = ErrorMessages.MsgTokenMinLength)]
    public string? SessionToken { get; set; }

    [Required(ErrorMessage = ErrorMessages.MsgRegisterDateRequired)]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = ConstExpressions.DateTimeFormat, ApplyFormatInEditMode = true)]
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

    [Required]
    public TKey? UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual T? User { get; set; }
}