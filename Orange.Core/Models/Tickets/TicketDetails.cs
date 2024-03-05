namespace Orange_Bay.Models.Tickets;

public class TicketDetails
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string FamilyName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public DateTime CreatedOn { get; set; }

    public TicketType TicketType { get; set; }
    public int TicketTypeId { get; set; }

    public ICollection<TicketImage> TicketImages { get; set; } = new HashSet<TicketImage>();
}