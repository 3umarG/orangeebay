using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Auth;
using Orange_Bay.Utils;
using Serilog;

namespace Orange.EF.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Jwt _jwt;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly ImageSaver _imageSaver;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IOptions<Jwt> jwt,
        RoleManager<IdentityRole<int>> roleManager, IEmailService emailService, ImageSaver imageSaver)
    {
        _userManager = userManager;
        _jwt = jwt.Value;
        _roleManager = roleManager;
        _emailService = emailService;
        _imageSaver = imageSaver;
    }


    public async Task<AuthModelResponseDto> RegisterAsync(UserRegisterDto dto, string role = "user")
    {
        var authModel = new AuthModelResponseDto();
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is not null)
        {
            throw new CustomExceptionWithStatusCode(400, "There is already user with that Email !!");
        }

        var photoUrl = await GeneratePhotoUrlForUser(dto.Image);

        var appUser = new ApplicationUser
        {
            UserName = dto.UserName.Replace(" ", ""),
            Email = dto.Email,
            FullName = dto.FullName,
            CultureTypeId = 1, // TODO : will removed ...!!
            UserTypeId = dto.UserTypeId,
            PhotoUrl = photoUrl,
            PhoneNumber = dto.Phone ?? ""
        };

        var adminRoleExists = await _roleManager.RoleExistsAsync(role);
        if (!adminRoleExists)
        {
            await _roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        var result = await _userManager.CreateAsync(appUser, dto.Password);

        if (!result.Succeeded)
        {
            throw new CustomExceptionWithStatusCode(
                400,
                $"Something went wrong {result.Errors.First().Description} , please try again !!"
            );
        }

        await _userManager.AddToRoleAsync(appUser, role);
        await InitializeSuccessAuthModel(authModel, appUser);

        return authModel;
    }

    private async Task<string?> GeneratePhotoUrlForUser(IFormFile? image)
    {
        string? photoUrl = null;
        if (image is not null)
        {
            photoUrl = await _imageSaver.GenerateImageUrl(image, "auth");
        }

        return photoUrl;
    }

    private async Task InitializeSuccessAuthModel(AuthModelResponseDto authModel, ApplicationUser appUser)
    {
        var jwtToken = await CreateJwtToken(appUser);

        await _userManager.UpdateAsync(appUser);

        authModel.IsAuthed = true;
        authModel.Email = appUser.Email!;
        authModel.FullName = appUser.FullName;
        authModel.PhotoUrl = appUser.PhotoUrl;

        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authModel.AccessTokenExpiration = jwtToken.ValidTo;
        authModel.Id = appUser.Id;
        authModel.UserTypeId = appUser.UserTypeId;
        authModel.Phone = appUser.PhoneNumber;
        authModel.UserName = appUser.UserName;
    }

    private async Task<SecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("uid", user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }

    public async Task<AuthModelResponseDto> LoginAsync(UserLoginDto dto)
    {
        var auth = new AuthModelResponseDto();

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            throw new CustomExceptionWithStatusCode(404, "Not Found User or not correct password !!");
        }

        auth.FullName = user.FullName;
        auth.Email = user.Email!;
        auth.IsAuthed = true;
        auth.PhotoUrl = user.PhotoUrl;
        auth.Id = user.Id;
        auth.UserTypeId = user.UserTypeId;
        auth.Phone = user.PhoneNumber;
        auth.UserName = user.UserName;


        var jwtToken = await CreateJwtToken(user);
        auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        auth.AccessTokenExpiration = jwtToken.ValidTo;

        return auth;
    }

    public async Task<AuthModelResponseDto> ResetClientPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            throw new CustomExceptionWithStatusCode(404, "Not Found User with that Email !!");
        }

        var newPassword = GeneratePassword();

        var authModel = await ResetPasswordAsync(newPassword, user);

        await _emailService.SendEmail(user, newPassword);


        return authModel;
    }

    public async Task<AuthModelResponseDto> ResetClientPasswordAsync(ResetPasswordDto dto, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            throw new CustomExceptionWithStatusCode(404, "Not Found User with that Email !!");
        }

        var authModel = await ResetPasswordAsync(newPassword, user);

        return authModel;
    }

    private static string GeneratePassword()
    {
        var options = new PasswordOptions
        {
            RequiredLength = 8,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = true,
            RequireNonAlphanumeric = true
        };

        var random = new Random();
        var password = new StringBuilder();

        while (password.Length < options.RequiredLength ||
               (options.RequireDigit && !password.ToString().Any(char.IsDigit)) ||
               (options.RequireLowercase && !password.ToString().Any(char.IsLower)) ||
               (options.RequireUppercase && !password.ToString().Any(char.IsUpper)) ||
               (options.RequireNonAlphanumeric && !password.ToString().Any(IsNonAlphanumeric)))
        {
            var nextChar = (char)random.Next(33, 126);

            if (nextChar == '"')
                continue;

            password.Append(nextChar);
        }

        Log.Information("Finishing generating new password ....");
        return password.ToString();
    }

    private static bool IsNonAlphanumeric(char c)
    {
        return !char.IsLetterOrDigit(c);
    }

    private async Task<AuthModelResponseDto> ResetPasswordAsync(string newPassword, ApplicationUser user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var temp = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!temp.Succeeded)
        {
            var message = "";

            foreach (var error in temp.Errors)
            {
                message += error.Description + " , ";
            }

            throw new CustomExceptionWithStatusCode(400, message);
        }

        var authModel = new AuthModelResponseDto();
        var jwtToken = await CreateJwtToken(user);
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authModel.AccessTokenExpiration = jwtToken.ValidTo;

        authModel.IsAuthed = true;
        authModel.Email = user.Email;
        authModel.FullName = user.FullName;
        authModel.Id = user.Id;
        authModel.PhotoUrl = user.PhotoUrl;
        authModel.Phone = user.PhoneNumber;
        authModel.UserName = user.UserName;
        authModel.UserTypeId = user.UserTypeId;

        Log.Information($"Reset Password Successfully for {user.Email} ...");
        return authModel;
    }

    public async Task<ApplicationUser> FindUserByIdAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User with id : {id}");
        }

        return user;
    }

    public async Task<int> FindUserTypeIdByUserAsync(int userId)
    {
        // there is no authorization token (anonymous)
        // default user type id = 1 (guest)
        if (userId == AppUtils.NotExistUserId)
        {
            return 1;
        }

        // there is 
        var user = await _userManager.Users
            .Where(user => user.Id == userId)
            .Select(user => new { user.Id, user.UserTypeId })
            .FirstOrDefaultAsync();

        if (user is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User with Id : {userId}");
        }

        return user.UserTypeId;
    }

    public async Task<bool> AnyUserWithIdAsync(int id)
    {
        var hasUserId = await _userManager.Users.AnyAsync(user => user.Id == id);
        return hasUserId;
    }

    public async Task<bool> IsCorrectUserPasswordAsync(int userId, string password)
    {
        var user = await FindUserByIdAsync(userId);
        return await _userManager.CheckPasswordAsync(user, password);
    }
}