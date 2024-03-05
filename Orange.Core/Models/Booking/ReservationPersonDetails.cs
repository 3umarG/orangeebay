using System.Text.Json.Serialization;

namespace Orange_Bay.Models.Booking;

public class ReservationPersonDetails
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public PersonType Type { get; set; }


    public int ReservationId { get; set; }
    [JsonIgnore] public Reservation Reservation { get; set; }
}

public enum PersonType
{
    Adult = 1,
    Child = 0
}