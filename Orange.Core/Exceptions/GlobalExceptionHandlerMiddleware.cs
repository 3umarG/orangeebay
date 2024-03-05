using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Orange_Bay.DTOs;
using Orange_Bay.DTOs.Auth;
using Serilog;

namespace Orange_Bay.Exceptions;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CustomExceptionWithStatusCode e)
        {
            _logger.LogInformation(
                "`CustomExceptionWithStatusCode` handled with Status Code : #{code} --- Message : #{message} --- Source : #{source}",
                e.StatusCode, e.Message, e.Source);
            context.Response.StatusCode = e.StatusCode;

            await context.Response.WriteAsJsonAsync(
                new ResponseModelDto<object?>(
                    false,
                    e.Message,
                    e.StatusCode,
                    null)
            );
        }
        catch (DbUpdateException e)
        {
            _logger.LogWarning(
                "`DbUpdateException` handled with Status Code : #{code} --- Message : #{message} --- Source : #{source}",
                405, e.Message, e.Source);

            context.Response.StatusCode = 405;
            await context.Response.WriteAsJsonAsync(
                new ResponseModelDto<object?>(
                    false,
                    $"Exception in DB : {e.Message}",
                    405,
                    null
                ));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "`Global Exception` handled with Status Code : #{code} --- Message : #{message} --- Source : #{source} --- StackTrace : #{trace}",
                500, ex.Message, ex.Source, ex.StackTrace);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(
                new ResponseModelDto<object?>(
                    false,
                    $"INTERNAL SERVER ERROR : {ex.Message}",
                    500,
                    null
                ));
        }
    }

    private static async Task HandleException(Exception ex, HttpContext httpContext)
    {
        if (ex is CustomExceptionWithStatusCode customExceptionWithStatusCode)
        {
            httpContext.Response.StatusCode = customExceptionWithStatusCode.StatusCode;

            await httpContext.Response.WriteAsJsonAsync(
                new ResponseModelDto<object?>(
                    false,
                    customExceptionWithStatusCode.Message,
                    customExceptionWithStatusCode.StatusCode,
                    null)
            );
        }
        else
        {
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsJsonAsync(
                new ResponseModelDto<object?>(
                    false,
                    "INTERNAL SERVER ERROR !!",
                    500,
                    null
                ));
        }
    }
}