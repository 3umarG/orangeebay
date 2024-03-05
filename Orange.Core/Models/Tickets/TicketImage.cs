using System.Text.Json.Serialization;

namespace Orange_Bay.Models.Tickets;

public class TicketImage
{
    [JsonIgnore] public int Id { get; set; }
    public string PhotoUrl { get; set; }
    public string TicketId { get; set; }

    [JsonIgnore] public int TicketDetailsId { get; set; }
    [JsonIgnore] public TicketDetails TicketDetails { get; set; }
}