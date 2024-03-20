namespace AE3803Notification.Domain.User;

public class UserTransferCompleteDto
{
    public Guid TicketId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
