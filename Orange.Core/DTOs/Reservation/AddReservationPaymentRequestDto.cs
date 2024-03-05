namespace Orange_Bay.DTOs.Reservation;

public record AddReservationPaymentRequestDto(
    int ReservationId,
    string TransactionId
);