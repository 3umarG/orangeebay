using System.IdentityModel.Tokens.Jwt;

namespace Orange_Bay.Utils;

public static class AppUtils
{
    private const string ApplicationHost = "https://ommargomma-001-site1.atempurl.com/api/";
    private const string LocalApplicationHost = "https://localhost:9999/api/";
    public const string UsersImagesPath = ApplicationHost + "Images/Users/";
    public const string ProgramsImagesPath = ApplicationHost + "Images/Programs/";
    public const string GalleryImagesPath = ApplicationHost + "Images/Gallery/";
    public const string DiningImagesPath = ApplicationHost + "Images/Dining/";
    public const string SliderImagesPath = ApplicationHost + "Images/Slider/";
    public const string TicketImagesPath = ApplicationHost + "Images/Tickets/";
    public const string CompaniesImagesPath = ApplicationHost + "Images/Company/";
    public const int NumberOfItemsPerPage = 10;
    public const int NumberOfReviewsPerPage = 5;
    public const int NumberOfReservationsPerPage = 10;
    public const int NumberOfUsersPerPage = 10;
    public const int NotExistUserId = -1;
    public const int NumberOfAdditionalServicesPerPage = 5;
    public const int NumberOfTicketsPerPage = 5;
    public const int NumberOfProgramsPerPage = 5;
    public static readonly string RootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");


    public static string ExtractUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var email = jwtSecurityToken.Claims.First(claim => claim.Type == "uid").Value;
        return email;
    }
}