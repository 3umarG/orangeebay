using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange.EF.Services;

namespace Orange.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AboutUsController
{
    [HttpGet]
    public IActionResult GetAboutUs()
    {
        var result =  AboutUsService.GetAboutUs();
        var response = ResponseModelDto<string>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpPost]
    public IActionResult UpdateAboutUs([FromBody] AboutUsRequestBody requestBody)
    {
        AboutUsService.Update(requestBody.Text);
        var result = AboutUsService.GetAboutUs();
        var response = ResponseModelDto<string>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

}

public class AboutUsRequestBody
{
    public string Text { get; set; }
}