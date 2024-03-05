using Orange_Bay.Models.Auth;
using Orange_Bay.Models.Programs;

namespace Orange_Bay.Models.Booking;

public class Reservation
{
    public int Id { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime BookedOn { get; set; }
    public DateTime? CancellationDeadlineDate { get; set; }
    public bool IsPaid { get; set; } = false;
    public int ProgramId { get; set; }
    public Program Program { get; set; }

    public ApplicationUser User { get; set; }
    public int UserId { get; set; }

    public int NumberOfAdults { get; set; }
    public double PricePerAdult { get; set; }
    public double AdultTotalPrice => NumberOfAdults * PricePerAdult;

    public int NumberOfChild { get; set; }
    public double PricePerChild { get; set; }
    public double ChildTotalPrice => NumberOfChild * PricePerChild;

    public double TotalProgramPrice => ChildTotalPrice + AdultTotalPrice;

    public double? TotalAdditionalServicesPrice =>
        ReservationAdditionalServices?.Sum(service =>
            (service.NumberOfChild * service.PricePerChild) +
            (service.NumberOfAdults * service.PricePerAdult));

    public double TotalReservationPrice => TotalAdditionalServicesPrice + TotalProgramPrice ?? 0;


    public bool IsCancelled { get; set; }

    public bool IsActive => !IsCancelled && BookingDate > DateTime.Now;

    public bool CanBeCancelOrEdit => !IsPaid && IsActive && DateTime.Today <= CancellationDeadlineDate;

    public virtual ICollection<ReservationAdditionalService>? ReservationAdditionalServices { get; set; } =
        new HashSet<ReservationAdditionalService>();

    public virtual ICollection<ReservationPersonDetails>? ReservationPersonsDetails { get; set; } =
        new HashSet<ReservationPersonDetails>();

    public AttendanceStatus AttendanceStatus { get; set; }

    public bool IsAttended => AttendanceStatus == AttendanceStatus.Attended;
    public bool IsMissed => AttendanceStatus == AttendanceStatus.Pending && BookingDate < DateTime.Today;

    public ReservationPaymentDetails PaymentDetails { get; set; }
}

public enum AttendanceStatus
{
    Pending,
    Attended
}