namespace Orange_Bay.DTOs.Dashboard;

public record DashboardDailyOverviewResponseDto(
    int NumberOfAdults,
    int NumberOfChild,
    double TotalAdultsPrice,
    double TotalChildPrice,
    int TotalNumberOfAdditionalServices,
    double TotalPriceOfAdditionalService,
    int TotalNumberOfBookingByCompany,
    int TotalNumberOfBookingByGuest,
    int TotalNumberOfBookingByEmployee,
    double TotalPriceOfReservationsByCompany,
    double TotalPriceOfReservationsByGuest,
    double TotalPriceOfReservationsByEmployee,
    int TotalNumberOfBookingCancellation,
    double TotalPriceOfBookingCancellation,
    double TotalPriceOfDay
);