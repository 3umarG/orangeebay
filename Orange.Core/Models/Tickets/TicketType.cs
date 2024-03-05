using System.Text.Json.Serialization;

namespace Orange_Bay.Models.Tickets;

public class TicketType
{
    public int Id { get; set; }
    public string Type { get; set; }

    [JsonIgnore] public ICollection<TicketDetails> Tickets { get; set; } = new HashSet<TicketDetails>();
}