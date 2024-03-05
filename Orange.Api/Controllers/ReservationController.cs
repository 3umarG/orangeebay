using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.Reservation;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Booking;
using Orange_Bay.Utils;
using Orange.EF;
using Orange.EF.Services;

namespace Orange.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationController
{
    private readonly IReservationService _reservationService;

    public ReservationController(ApplicationDbContext context, IAuthService authService)
    {
        _reservationService = new ReservationService(authService, context);
    }

    [HttpPost]
    public async Task<IActionResult> AddReservationAsync([FromBody] AddReservationRequestDto dto)
    {
        var result = await _reservationService.AddReservationAsync(dto);
        var responseBody = ResponseModelDto<ReservationResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserReservationsByPaymentStatus(int userId,
        ReservationStatus reservationStatus)
    {
        var result = await _reservationService.GetAllReservationsByUserId(reservationStatus, userId);
        var responseBody = ResponseModelDto<List<ReservationResponseDto>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> CancelReservationByIdAsync(int id)
    {
        var result = await _reservationService.CancelReservationByIdAsync(id);
        var responseBody = ResponseModelDto<ReservationResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateReservationAsync(int id, AddReservationRequestDto dto)
    {
        var result = await _reservationService.UpdateReservationAsync(id, dto);
        var responseBody = ResponseModelDto<ReservationResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpPost("Payment")]
    public async Task<IActionResult> AddReservationPaymentDetailsAsync(AddReservationPaymentRequestDto dto)
    {
        var result = await _reservationService.AddReservationPaymentAsync(dto);
        var responseBody = ResponseModelDto<ReservationPaymentDetails>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }
}