using Microsoft.EntityFrameworkCore;
using Orange_Bay.DTOs.Programs;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Wishlist;
using Serilog;

namespace Orange.EF.Services;

public class WishlistsService : IWishlistsService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAuthService _authService;

    public WishlistsService(ApplicationDbContext dbContext, IAuthService authService)
    {
        _dbContext = dbContext;
        _authService = authService;
    }

    public async Task<bool> AddProgramToUserWishlistsAsync(int userId, int programId)
    {
        var user = await _authService.FindUserByIdAsync(userId);
        var program = await _dbContext.Programs.FindAsync(programId);


        if (user is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User with id : {userId}");
        }

        if (program is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program with id : {programId}");
        }

        var alreadyProgramAtWishlist =
            await _dbContext.ProgramWishlists
                .FirstOrDefaultAsync(w => w.ProgramId == programId && w.UserId == userId);

        if (alreadyProgramAtWishlist is not null)
        {
            // remove from wishlist
            _dbContext.ProgramWishlists.Remove(alreadyProgramAtWishlist);
        }
        else
        {
            // add it to wishlist
            var programToWishlist = new ProgramWishlist
            {
                Program = program,
                User = user
            };

            await _dbContext.ProgramWishlists.AddAsync(programToWishlist);
        }

        await _dbContext.SaveChangesAsync();


        return true;
    }

    public async Task<List<ProgramResponseDto>> GetWishlistsForUserAsync(int userId)
    {
        var user = await _authService.FindUserByIdAsync(userId);
        if (user is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User with id : {userId}");
        }


        var programs = await _dbContext.ProgramWishlists
            .Include(pw => pw.Program)
            .ThenInclude(p => p.ProgramPrices)
            .Include(pw => pw.Program)
            .ThenInclude(p => p.ProgramImages)
            .Include(pw => pw.Program)
            .ThenInclude(p => p.ProgramReviews)
            .AsSplitQuery()
            .Where(p => p.UserId == userId)
            .Select(pw => pw.Program)
            .ToListAsync();

        return programs.Select(p =>
        {
            var averageReviews = 0.0;
            if (p.ProgramReviews != null && p.ProgramReviews.Count != 0)
            {
                averageReviews = p.ProgramReviews
                    .Average(review => review.RateFromFive);
                Log.Information("Finish Average");
            }


            var images = new List<string>();
            if (p.ProgramImages != null && p.ProgramImages.Count != 0)
            {
                images = p.ProgramImages.Select(programImage => programImage.PhotoUrl).ToList();
                Log.Information("Finish Images");
            }

            var pricePerChild = 0.0;
            var pricePerAdult = 0.0;
            if (p.ProgramPrices != null && p.ProgramPrices.Count != 0)
            {
                pricePerChild = p.ProgramPrices
                    .Where(price => price.UserTypeId == user.UserTypeId)
                    .Select(price => price.PricePerChild)
                    .FirstOrDefault();
                Log.Information("Finish Child Prices");

                pricePerAdult = p.ProgramPrices
                    .Where(price => price.UserTypeId == user.UserTypeId)
                    .Select(price => price.PricePerAdult)
                    .FirstOrDefault();
                Log.Information("Finish Adult Prices");
            }

            return new ProgramResponseDto(
                p.Id,
                images,
                p.Name,
                averageReviews,
                pricePerChild,
                pricePerAdult,
                true,
                null,
                null
            );
        }).ToList();
    }
}