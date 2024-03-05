using System.Text.Json.Serialization;

namespace Orange_Bay.Models.Booking;

public class ReservationPaymentDetails
{
    public int Id { get; set; }
    
    public int ReservationId { get; set; }
    [JsonIgnore] public Reservation Reservation { get; set; }
    public string TransactionId { get; set; }
    public DateTime PayedOn { get; set; }
    public double TotalPrice { get; set; }
}