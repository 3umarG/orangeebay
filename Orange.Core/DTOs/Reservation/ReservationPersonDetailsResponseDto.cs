namespace Orange_Bay.DTOs.Reservation;

public record ReservationPersonDetailsResponseDto(
    string Name,
    string Email,
    string Phone,
    string Type,
    int ReservationId
);