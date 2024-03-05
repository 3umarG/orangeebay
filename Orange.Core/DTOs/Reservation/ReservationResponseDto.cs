using Orange_Bay.DTOs.Programs;
using Orange_Bay.Models.Booking;

namespace Orange_Bay.DTOs.Reservation;

public record ReservationResponseDto(
    int ReservationId,
    DateTime BookingDate,
    DateTime BookedOn,
    DateTime? CancellationDeadline,
    bool CanBeCancelOrEdit,
    double TotalPrice,
    bool IsPaid,
    ReservationProgramResponseDto Program,
    List<ReservationAdditionalServiceResponseDto> Services,
    double TotalProgramPrice,
    double TotalServicesPrice,
    List<ReservationPersonDetailsResponseDto> Persons
);