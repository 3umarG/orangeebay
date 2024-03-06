using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.Dining;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Dining;
using Orange.EF;
using Orange.EF.Services;

namespace Orange.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class DiningController
{
    private readonly IDiningService _diningService;

    public DiningController(IDiningService diningService)
    {
        _diningService = diningService;
    }


    [HttpPost]
    public async Task<IActionResult> AddDiningItem([FromForm] DiningRequestDto requestDto)
    {
        var result = await _diningService.AddDiningItemAsync(requestDto);
        var response = ResponseModelDto<DiningItem>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDiningItems(int? diningCategoryId)
    {
        var result = await _diningService.GetAllDiningItemsAsync(diningCategoryId);
        var response = ResponseModelDto<List<DiningItem>>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDiningByIdAsync(int id)
    {
        var result = await _diningService.GetDiningByIdAsync(id);
        var response = ResponseModelDto<DiningItem>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDiningAsync(int id, [FromForm] DiningRequestDto requestDto)
    {
        var result = await _diningService.UpdateDiningById(id, requestDto);
        var response = ResponseModelDto<DiningItem>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDiningByIdAsync(int id)
    {
        var result = await _diningService.DeleteAsync(id);
        var response = ResponseModelDto<DiningItem>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }
}