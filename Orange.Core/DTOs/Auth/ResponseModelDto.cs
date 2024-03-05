namespace Orange_Bay.DTOs.Auth;

public record ResponseModelDto<T>(
    bool Success,
    string? Message,
    int StatusCode,
    T Data
)
{
    public static ResponseModelDto<T> BuildSuccessResponse(T data)
    {
        return new ResponseModelDto<T>(
            true,
            null,
            200,
            data
        );
    }
}