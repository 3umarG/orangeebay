using Orange_Bay.Models.AdditionalServices;
using Orange_Bay.Models.Auth;

namespace Orange_Bay.Models.Booking;

public class ReservationAdditionalService
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfAdults { get; set; }
    public double PricePerAdult { get; set; }

    public int NumberOfChild { get; set; }
    public double PricePerChild { get; set; }

    public double AdultTotalPrice => NumberOfAdults * PricePerAdult;
    public double ChildTotalPrice => NumberOfChild * PricePerChild;

    public double TotalServicePrice => ChildTotalPrice + AdultTotalPrice;

    public int AdditionalServiceId { get; set; }
    public AdditionalService AdditionalService { get; set; }

    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; }
}