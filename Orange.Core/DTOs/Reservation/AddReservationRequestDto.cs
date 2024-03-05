using System.ComponentModel.DataAnnotations;

namespace Orange_Bay.DTOs.Reservation;

public record AddReservationRequestDto(
    int UserId,
    int ProgramId,
    DateTime BookingDate,
    int NumberOfChild,
    int NumberOfAdults,
    List<AdditionalServiceRequestDto> AdditionalServices,
    List<ReservationPersonRequestDto> Persons
);