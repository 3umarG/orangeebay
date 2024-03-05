using Orange_Bay.DTOs.AdditionalServices;
using Orange_Bay.Models.Booking;

namespace Orange_Bay.DTOs.Dashboard;

public record DashboardReservationDetailsResponseDto(
    int Id,
    string Program,
    string ClientName,
    string ClientPhone,
    string ClientEmail,
    string ClientType,
    List<ReservationPersonDetails> Persons,
    DateTime BookingDate,
    DateTime BookedOn,
    bool IsCancelled,
    string AttendanceStatus,
    bool IsMissed,
    double TotalProgramPrice,
    double TotalAdditionalServicesPrice,
    double TotalReservationPrice,
    int NumberOfAdults,
    int NumberOfChilds,
    List<AdditionalServiceResponseDto> Services);