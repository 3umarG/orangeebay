using Microsoft.AspNetCore.Http;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Utils;

namespace Orange.EF.Services;

public class TokenService : ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int ExtractUserIdFromToken()
    {
        var fullBearerToken = ExtractFullBearerTokenFromRequestAuthorizationHeader();

        if (fullBearerToken is null)
        {
            return AppUtils.NotExistUserId;
        }

        var jwtToken = ExtractJwtTokenFromFullBearerToken(fullBearerToken);
        var userId = AppUtils.ExtractUserIdFromToken(jwtToken);
        return int.Parse(userId);
    }

    private static string ExtractJwtTokenFromFullBearerToken(string fullBearerToken)
    {
        return fullBearerToken.Split(" ")[1];
    }

    private string? ExtractFullBearerTokenFromRequestAuthorizationHeader()
    {
        return _httpContextAccessor.HttpContext?
            .Request.Headers.Authorization.FirstOrDefault();
    }
}