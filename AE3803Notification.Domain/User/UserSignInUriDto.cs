namespace AE3803Notification.Domain.User;

public class UserSignInUriDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordSetUri { get; set; }
    public string VerificationEmailUri { get; set; }
}
