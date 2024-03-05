using Orange_Bay.DTOs.Reservation;
using Orange_Bay.Models.Booking;
using Orange_Bay.Utils;

namespace Orange_Bay.Interfaces.Services;

public interface IReservationService
{
    Task<ReservationResponseDto> AddReservationAsync(AddReservationRequestDto dto);

    Task<List<ReservationResponseDto>> GetAllReservationsByUserId(ReservationStatus reservationStatus, int userId);
    Task<ReservationResponseDto> CancelReservationByIdAsync(int reservationId);
    Task<ReservationResponseDto> UpdateReservationAsync(int reservationId, AddReservationRequestDto dto);
    Task<ReservationPaymentDetails> AddReservationPaymentAsync(AddReservationPaymentRequestDto dto);
}