using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.Dashboard;
using Orange_Bay.DTOs.Profile;
using Orange_Bay.DTOs.Programs.Request;
using Orange_Bay.DTOs.Shared;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.AdditionalServices;
using Orange_Bay.Models.Auth;
using Orange_Bay.Models.CompanyImages;
using Orange_Bay.Models.Programs;
using Orange_Bay.Models.SliderImage;
using Orange_Bay.Models.Tickets;

namespace Orange.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController
{
    private readonly IDashboardService _dashboardService;
    private readonly ISliderImagesService _sliderImagesService;
    private readonly ITicketsService _ticketsService;
    private readonly ICompaniesImagesService _companiesImagesService;

    public DashboardController(IDashboardService dashboardService, ISliderImagesService sliderImagesService, ITicketsService ticketsService, ICompaniesImagesService companiesImagesService)
    {
        _dashboardService = dashboardService;
        _sliderImagesService = sliderImagesService;
        _ticketsService = ticketsService;
        _companiesImagesService = companiesImagesService;
    }

    [HttpGet("Overview")]
    public async Task<IActionResult> GetOverviewForDateAsync(DateTime date)
    {
        var result = await _dashboardService.GetDashboardDailyOverviewAsync(date);
        var responseBody = ResponseModelDto<DashboardDailyOverviewResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("Reservations")]
    public async Task<IActionResult> GetReservationsOverviewAsync(DateTime date, [FromQuery] int page = 1)
    {
        var result = await _dashboardService.GetReservationsOverviewAsync(date, page);
        var responseBody = ResponseModelDto<PaginatedResponseDto<DashboardReservationOverviewResponseDto>>
            .BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("Reservations-Weekly")]
    public async Task<IActionResult> GetWeeklyReservationsOverviewAsync(DateTime from, DateTime to, int page = 1)
    {
        var result = await _dashboardService.GetWeeklyReservationsOverviewAsync(from, to, page);
        var responseBody = ResponseModelDto<PaginatedResponseDto<DashboardReservationOverviewResponseDto>>
            .BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("Reservations/{id:int}")]
    public async Task<IActionResult> GetReservationDetailsByIdAsync(int id)
    {
        var result = await _dashboardService.GetReservationDetailsByIdAsync(id);
        var responseBody = ResponseModelDto<DashboardReservationDetailsResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpPut("Reservations/Attendance/{id:int}")]
    public async Task<IActionResult> ApplyReservationAttendanceAsync(int id)
    {
        var result = await _dashboardService.ApplyReservationAttendanceAsync(id);
        var responseBody = ResponseModelDto<bool>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpPost("Auth/Register")]
    public async Task<IActionResult> RegisterAsync([FromBody] DashboardRegisterRequestDto dto)
    {
        var result = await _dashboardService.RegisterOnDashboardAsync(dto);
        var responseBody = ResponseModelDto<DashboardAuthModelResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpPost("Auth/Login")]
    public async Task<IActionResult> LoginAsync([FromBody] DashboardLoginRequestDto dto)
    {
        var result = await _dashboardService.LoginAsync(dto);
        var responseBody = ResponseModelDto<DashboardAuthModelResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("Monthly-Statistics")]
    public async Task<IActionResult> GetMonthlyStatisticsAsync([FromQuery] int year, [FromQuery] int month)
    {
        var result = await _dashboardService.GetMonthlyReservationsStatisticsAsync(year, month);
        var responseBody = ResponseModelDto<List<DailyStaticsResponseDto>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpPost("AdditionalServices")]
    public async Task<IActionResult> AddAdditionalServicesAsync([FromBody] DashboardAdditionalServiceRequestDto dto)
    {
        var result = await _dashboardService.AddAdditionalServiceAsync(dto);
        var responseBody = ResponseModelDto<AdditionalService>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("AdditionalServices")]
    public async Task<IActionResult> GetAllAdditionalServicesWithAllPricesAsync([FromQuery] int page = 1)
    {
        var result = await _dashboardService.GetAllAdditionalServicesWithAllPricesAsync(page);
        var responseBody = ResponseModelDto<PaginatedResponseDto<AdditionalService>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpDelete("AdditionalServices/{id:int}")]
    public async Task<IActionResult> DeleteAdditionalServiceByIdAsync(int id)
    {
        var result = await _dashboardService.DeleteAdditionalServiceByIdAsync(id);
        var responseBody = ResponseModelDto<AdditionalService>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpPut("AdditionalServices/{id:int}")]
    public async Task<IActionResult> UpdateAdditionalServiceByIdAsync(int id,
        [FromBody] DashboardAdditionalServiceRequestDto dto)
    {
        var result = await _dashboardService.UpdateAdditionalServiceByIdAsync(id, dto);
        var responseBody = ResponseModelDto<AdditionalService>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpPost("Programs")]
    public async Task<IActionResult> AddProgramAsync([FromBody] DashboardProgramRequestDto dto)
    {
        var result = await _dashboardService.AddProgramAsync(dto);
        var responseBody = ResponseModelDto<Orange_Bay.Models.Programs.Program>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("Programs")]
    public async Task<IActionResult> GetAllProgramsAsync([FromQuery] int page = 1)
    {
        var result = await _dashboardService.GetAllProgramsAsync(page);
        var responseBody = ResponseModelDto<PaginatedResponseDto<Orange_Bay.Models.Programs.Program>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpDelete("Programs/Images/{id:int}")]
    public async Task<IActionResult> DeleteProgramImageByIdAsync(int id)
    {
        var result = await _dashboardService.DeleteProgramImageByIdAsync(id);
        var responseBody = ResponseModelDto<ProgramImage>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpDelete("Programs/{id:int}")]
    public async Task<IActionResult> DeleteProgramByIdAsync(int id)
    {
        var result = await _dashboardService.DeleteProgramByIdAsync(id);
        var responseBody = ResponseModelDto<bool>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpPut("Programs/{id:int}")]
    public async Task<IActionResult> UpdateProgramByIdAsync(int id, DashboardProgramRequestDto dto)
    {
        var result = await _dashboardService.UpdateProgramByIdAsync(id, dto);
        var responseBody = ResponseModelDto<Orange_Bay.Models.Programs.Program>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("UserTypes")]
    public async Task<IActionResult> GetAllUserTypesAsync()
    {
        var result = await _dashboardService.GetAllUserTypesAsync();
        var responseBody = ResponseModelDto<List<UserType>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("Programs/{id:int}")]
    public async Task<IActionResult> GetProgramByIdAsync(int id)
    {
        var result = await _dashboardService.GetProgramByIdAsync(id);
        var responseBody = ResponseModelDto<DashboardProgramResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("AdditionalServices/{id:int}")]
    public async Task<IActionResult> GetAdditionalServiceByIdAsync(int id, [FromQuery] int page)
    {
        var result = await _dashboardService.GetAdditionalServiceByIdAsync(id);
        var responseBody = ResponseModelDto<AdditionalService>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("Users")]
    public async Task<IActionResult> GetAllUsersWithUserTypeId([FromQuery] int? userTypeId = null, [FromQuery] int page = 1)
    {
        var result = await _dashboardService.GetAllUsersByUserTypeIdAsync(userTypeId, page);
        var responseBody = ResponseModelDto<PaginatedResponseDto<ProfileResponseDto>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpPost("SliderImages")]
    public async Task<IActionResult> AddSliderImagesAsync([FromForm] ImagesRequestDto requestDto)
    {
        var result = await _sliderImagesService.AddSliderImagesAsync(requestDto);
        var responseBody = ResponseModelDto<List<SliderImage>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpDelete("SliderImages/{id:int}")]
    public async Task<IActionResult> DeleteSliderImageByIdAsync(int id)
    {
        var result = await _sliderImagesService.DeleteSliderImageAsync(id);
        var responseBody = ResponseModelDto<SliderImage>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }
    
    [HttpPost("CompaniesImages")]
    public async Task<IActionResult> AddCompaniesImagesAsync([FromForm] ImagesRequestDto requestDto)
    {
        var result = await _companiesImagesService.AddCompanyImagesAsync(requestDto);
        var responseBody = ResponseModelDto<List<CompanyImage>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpDelete("CompaniesImages/{id:int}")]
    public async Task<IActionResult> DeleteCompanyImageByIdAsync(int id)
    {
        var result = await _companiesImagesService.DeleteCompanyImageAsync(id);
        var responseBody = ResponseModelDto<CompanyImage>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("Tickets")]
    public async Task<IActionResult> GetAllTicketsDetailsAsync([FromQuery] DateTime? date=null , [FromQuery] int page = 1)
    {
        var result = await _ticketsService.GetAllTicketsDetailsAsync(page, date);
        var responseBody = ResponseModelDto<PaginatedResponseDto<TicketDetails>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }
}