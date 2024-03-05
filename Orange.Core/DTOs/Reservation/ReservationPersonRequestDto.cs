namespace Orange_Bay.DTOs.Reservation;

public record ReservationPersonRequestDto(
    string Name,
    string Email,
    string Phone,
    int Type
);