namespace Orange_Bay.DTOs.Reservation;

public record ReservationAdditionalServiceResponseDto(
    int ServiceId,
    string Name,
    int NumberOfChild,
    double TotalChildPrice,
    int NumberOfAdults,
    double TotalAdultsPrice
);