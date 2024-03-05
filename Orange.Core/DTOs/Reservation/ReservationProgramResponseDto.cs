namespace Orange_Bay.DTOs.Reservation;

public record ReservationProgramResponseDto(
    int ProgramId,
    string Name,
    List<string> Photos,
    int NumberOfChild,
    double TotalChildPrice,
    int NumberOfAdults,
    double TotalAdultsPrice
);