using Orange_Bay.DTOs;
using Orange_Bay.DTOs.Dashboard;
using Orange_Bay.DTOs.Profile;
using Orange_Bay.DTOs.Shared;
using Orange_Bay.Models.AdditionalServices;
using Orange_Bay.Models.Auth;
using Orange_Bay.Models.Programs;

namespace Orange_Bay.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardDailyOverviewResponseDto> GetDashboardDailyOverviewAsync(DateTime date);

    Task<PaginatedResponseDto<DashboardReservationOverviewResponseDto>> GetReservationsOverviewAsync(DateTime date,
        int page);

    Task<DashboardReservationDetailsResponseDto> GetReservationDetailsByIdAsync(int id);
    Task<DashboardAuthModelResponseDto> RegisterOnDashboardAsync(DashboardRegisterRequestDto dto);
    Task<DashboardAuthModelResponseDto> LoginAsync(DashboardLoginRequestDto dto);
    Task<List<DailyStaticsResponseDto>> GetMonthlyReservationsStatisticsAsync(int year, int month);
    Task<AdditionalService> AddAdditionalServiceAsync(DashboardAdditionalServiceRequestDto dto);
    Task<PaginatedResponseDto<AdditionalService>> GetAllAdditionalServicesWithAllPricesAsync(int page);
    Task<AdditionalService> DeleteAdditionalServiceByIdAsync(int id);
    Task<AdditionalService> UpdateAdditionalServiceByIdAsync(int id, DashboardAdditionalServiceRequestDto dto);
    Task<Program> AddProgramAsync(DashboardProgramRequestDto dto);
    Task<PaginatedResponseDto<Program>> GetAllProgramsAsync(int page);
    Task<bool> DeleteProgramByIdAsync(int id);
    Task<Program> UpdateProgramByIdAsync(int id, DashboardProgramRequestDto dto);
    Task<List<UserType>> GetAllUserTypesAsync();
    Task<DashboardProgramResponseDto> GetProgramByIdAsync(int id);
    Task<AdditionalService> GetAdditionalServiceByIdAsync(int id);
    Task<PaginatedResponseDto<ProfileResponseDto>> GetAllUsersByUserTypeIdAsync(int? userTypeId, int page);
    Task<PaginatedResponseDto<DashboardReservationOverviewResponseDto>> GetWeeklyReservationsOverviewAsync(DateTime from, DateTime to, int page);
    Task<ProgramImage> DeleteProgramImageByIdAsync(int id);
    Task<bool> ApplyReservationAttendanceAsync(int id);
}