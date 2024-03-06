using Microsoft.EntityFrameworkCore;
using Orange_Bay.DTOs.Programs;
using Orange_Bay.DTOs.Programs.Request;
using Orange_Bay.DTOs.Shared;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Repositories;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Programs;
using Orange_Bay.Utils;
using Orange.EF.Repositories.Impl;
using Serilog;

namespace Orange.EF.Services;

public class ProgramsService : IProgramsService
{
    private readonly IBaseRepository<Program> _programsRepository;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IAuthService _authService;
    private readonly ImageSaver _imageSaver;

    public ProgramsService(ApplicationDbContext context, IAuthService authService, ImageSaver imageSaver)
    {
        _applicationDbContext = context;
        _authService = authService;
        _imageSaver = imageSaver;
        _programsRepository = new ProgramsRepository(context);
    }


    public async Task<List<ProgramResponseDto>> GetAllProgramsByUserTypeAsync(int? userId, DateTime? date)
    {
        var userTypeId = 1;
        if (userId != null)
        {
            userTypeId = await _authService.FindUserTypeIdByUserAsync(userId.Value);
        }

        Log.Information("Start Queryable");
        var programs = await _programsRepository.QueryableOf()
            .Include(program => program.ProgramImages)
            .Include(program => program.ProgramPrices)
            .Include(program => program.ProgramReviews)
            .Where(program => date.HasValue
                ? program.ProgramPrices != null
                  && program.ProgramPrices.Any(price =>
                      (price.UserTypeId == userTypeId && price.FromDate <= date && date <= price.ToDate))
                : program.ProgramPrices != null
                  && program.ProgramPrices.Any(price =>
                      price.UserTypeId == userTypeId))
            .OrderByDescending(program => program.Id)
            .AsSplitQuery()
            .ToListAsync();
        Log.Information("Finish Queryable");

        IEnumerable<Task<ProgramResponseDto>> result = programs.Select(async p =>
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
            var fromDate = DateTime.Today;
            var toDate = DateTime.Today;
            if (p.ProgramPrices != null && p.ProgramPrices.Count != 0)
            {
                pricePerChild = p.ProgramPrices
                    .Where(price => price.UserTypeId == userTypeId)
                    .Select(price => price.PricePerChild)
                    .FirstOrDefault();
                Log.Information("Finish Child Prices");

                pricePerAdult = p.ProgramPrices
                    .Where(price => price.UserTypeId == userTypeId)
                    .Select(price => price.PricePerAdult)
                    .FirstOrDefault();
                Log.Information("Finish Adult Prices");

                fromDate = p.ProgramPrices
                    .Where(price => price.UserTypeId == userTypeId)
                    .Select(price => price.FromDate)
                    .FirstOrDefault();

                toDate = p.ProgramPrices
                    .Where(price => price.UserTypeId == userTypeId)
                    .Select(price => price.ToDate)
                    .FirstOrDefault();
            }

            var isFav = false;

            if (userId != null)
            {
                isFav = await _applicationDbContext.ProgramWishlists
                    .AnyAsync(pw =>
                        pw.ProgramId == p.Id
                        && pw.UserId == userId);
            }


            return new ProgramResponseDto(
                p.Id,
                images,
                p.Name,
                averageReviews,
                pricePerChild,
                pricePerAdult,
                isFav,
                fromDate,
                toDate
            );
        });

        return result.Select(p => p.Result).ToList();
    }

    private static bool IsBetweenDate(DateTime? date, DateTime fromDate, DateTime toDate)
    {
        return date >= fromDate && date <= toDate;
    }

    public async Task<ProgramOverviewResponseDto> GetProgramOverviewAsync(int programId, int? userId)
    {
        var userTypeId =
            userId is not null
                ? await _authService.FindUserTypeIdByUserAsync(userId.Value)
                : 1;

        var program = await FindProgramByIdAsync(programId, userTypeId);

        var pricePerChild = program.ProgramPrices!
            .FirstOrDefault(price => price.UserTypeId == userTypeId)!
            .PricePerChild;

        var pricePerAdult = program.ProgramPrices!
            .FirstOrDefault(price => price.UserTypeId == userTypeId)!
            .PricePerAdult;

        var fromDate = program.ProgramPrices!
            .FirstOrDefault(price => price.UserTypeId == userTypeId)!
            .FromDate;

        var toDate = program.ProgramPrices!
            .FirstOrDefault(price => price.UserTypeId == userTypeId)!
            .ToDate;


        return new ProgramOverviewResponseDto(
            program.Id,
            program.Name!,
            program.DurationInHours,
            program.Location,
            program.Description,
            program.InternalNotes,
            program.SpecialRequirements,
            pricePerChild,
            pricePerAdult,
            fromDate,
            toDate,
            program.ProgramImages?.Select(i => i.PhotoUrl).ToList() ?? new List<string>(),
            program.ProgramNotes?.Select(n => n.Note).ToList() ?? new List<string>(),
            program.MaxCapacity
        );
    }

    private async Task<Program> FindProgramByIdAsync(int programId, int userTypeId = 1)
    {
        var program =
            await _applicationDbContext.Programs
                .Include(program => program.ProgramImages)
                .Include(program => program.ProgramPrices)
                .Include(program => program.ProgramNotes)
                .AsSplitQuery()
                .Where(p =>
                    p.ProgramPrices != null
                    && p.ProgramPrices.Any(price => price.UserTypeId == userTypeId))
                .FirstOrDefaultAsync(p => p.Id == programId);

        if (program is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program with id : {programId}");
        }

        return program;
    }

    public async Task<List<ProgramPlanResponseDto>> GetProgramPlansAsync(int programId)
    {
        await CheckForProgramExistence(programId);

        return await _applicationDbContext.ProgramPlans
            .Where(p => p.ProgramId == programId)
            .OrderBy(p => p.Id)
            .Select(p => new ProgramPlanResponseDto(
                p.Time,
                p.Description
            ))
            .ToListAsync();
    }

    private async Task CheckForProgramExistence(int programId)
    {
        if (!await _applicationDbContext.Programs.AnyAsync(p => p.Id == programId))
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program with id : {programId}");
        }
    }

    public async Task<PaginatedResponseDto<ProgramReviewResponseDto>> GetProgramReviewsByIdAsync(int programId,
        int page)
    {
        await CheckForProgramExistence(programId);
        var reviews = page == 0
            ? await GetFullProgramReviewsAsync(programId)
            : await GetPaginatedProgramReviewsAsync(programId, page);

        var reviewsCount = await _applicationDbContext.ProgramReviews.Where(review => review.ProgramId == programId)
            .CountAsync();
        var pages = (int)Math.Ceiling((double)reviewsCount / AppUtils.NumberOfReviewsPerPage);

        return new PaginatedResponseDto<ProgramReviewResponseDto>
        {
            Items = reviews.Select(p => new ProgramReviewResponseDto(
                p.UserId,
                p.User.Email,
                p.User.FullName,
                p.User.PhotoUrl,
                p.RateFromFive,
                p.ReviewDescription,
                p.Date,
                p.Id
            )).ToList(),
            Pages = pages,
            CurrentPage = page
        };
    }

    private async Task<List<ProgramReview>> GetPaginatedProgramReviewsAsync(int programId, int page)
    {
        return await _applicationDbContext.ProgramReviews
            .Include(p => p.User)
            .Where(p => p.ProgramId == programId)
            .OrderByDescending(p => p.Id)
            .Skip((page - 1) * AppUtils.NumberOfReviewsPerPage)
            .Take(AppUtils.NumberOfReviewsPerPage)
            .ToListAsync();
    }

    private async Task<List<ProgramReview>> GetFullProgramReviewsAsync(int programId)
    {
        return await _applicationDbContext.ProgramReviews
            .Include(p => p.User)
            .Where(p => p.ProgramId == programId)
            .OrderByDescending(p => p.Id)
            .ToListAsync();
    }

    public async Task<ProgramReviewResponseDto> AddProgramReviewAsync(ProgramReviewRequestDto dto)
    {
        var user = await _authService.FindUserByIdAsync(dto.UserId);
        if (user is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User with id : {dto.UserId}");
        }

        if (!await _applicationDbContext.Programs.AnyAsync(p => p.Id == dto.ProgramId))
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program with ID : {dto.ProgramId}");
        }

        var programReview = new ProgramReview
        {
            Date = DateTime.Today,
            ProgramId = dto.ProgramId,
            User = user,
            RateFromFive = dto.RateFromFive,
            ReviewDescription = dto.Review
        };

        var savedProgramReview = (await _applicationDbContext.ProgramReviews.AddAsync(programReview)).Entity;
        await _applicationDbContext.SaveChangesAsync();

        return new ProgramReviewResponseDto(
            savedProgramReview.UserId,
            user.Email,
            user.FullName,
            user.PhotoUrl,
            savedProgramReview.RateFromFive,
            savedProgramReview.ReviewDescription,
            savedProgramReview.Date,
            savedProgramReview.Id
        );
    }

    public async Task<List<string>> AddProgramImagesAsync(int programId, ImagesRequestDto requestDto)
    {
        if (!await _applicationDbContext.Programs.AnyAsync(p => p.Id == programId))
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program with id : {programId}");
        }

        if (requestDto.Images.Count == 0) return new List<string>();

        var generatedImages = new List<ProgramImage>();
        foreach (var image in requestDto.Images)
        {
            var photoUrl = await _imageSaver.GenerateImageUrl(image, "programs");
            var programImage = new ProgramImage
            {
                ProgramId = programId,
                PhotoUrl = photoUrl
            };

            generatedImages.Add(programImage);
        }

        await _applicationDbContext.ProgramImages.AddRangeAsync(generatedImages);
        await _applicationDbContext.SaveChangesAsync();

        return generatedImages.Select(image => image.PhotoUrl).ToList();
    }

    public async Task<ProgramIncludedAndExcludedDetails> GetProgramIncludedAndExcludedDetails(int programId)
    {
        if (!await _applicationDbContext.Programs.AnyAsync(p => p.Id == programId))
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program with id : {programId}");
        }

        var includedDetails =
            await _applicationDbContext.ProgramIncludedDetails
                .Where(detail => detail.ProgramId == programId)
                .Select(p => p.Description)
                .ToListAsync();

        var excludedDetails =
            await _applicationDbContext.ProgramExcludedDetails
                .Where(detail => detail.ProgramId == programId)
                .Select(p => p.Description)
                .ToListAsync();

        return new ProgramIncludedAndExcludedDetails(includedDetails, excludedDetails);
    }
}