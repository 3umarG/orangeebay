namespace Orange_Bay.DTOs.Dashboard;

public record DashboardReservationOverviewResponseDto(
    int Id,
    string ClientName,
    string ClientType,
    string ProgramName,
    DateTime BookingDate,
    DateTime BookedOn,
    bool HasAdditionalServices,
    int NumberOfAdults,
    int NumberOfChilds,
    bool IsCancelled,
    string AttendanceStatus,
    bool IsMissed
);