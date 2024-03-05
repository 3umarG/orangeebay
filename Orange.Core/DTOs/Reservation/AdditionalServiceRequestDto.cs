namespace Orange_Bay.DTOs.Reservation;

public record AdditionalServiceRequestDto(
    int ServiceId,
    int NumberOfChild,
    int NumberOfAdults
);