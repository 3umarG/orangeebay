using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Orange_Bay.DTOs;
using Orange_Bay.DTOs.Programs.Request;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.CompanyImages;

namespace Orange.EF.Services;

public class CompaniesImagesService : ICompaniesImagesService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ImageSaver _imageSaver;

    public CompaniesImagesService(ApplicationDbContext dbContext, IOptions<AppSettings> appSettings, ImageSaver imageSaver)
    {
        _dbContext = dbContext;
        _imageSaver = imageSaver;
    }

    public async Task<List<CompanyImage>> AddCompanyImagesAsync(ImagesRequestDto imagesRequestDto)
    {
        if (imagesRequestDto.Images.Count == 0)
            return new List<CompanyImage>();

        var generatedImages = new List<CompanyImage>();


        await Task.WhenAll(imagesRequestDto.Images.Select(async imageFile =>
        {
            var imagePath = await _imageSaver.GenerateImageUrl(imageFile, "companies");
            var programImage = new CompanyImage
            {
                PhotoUrl = imagePath
            };

            generatedImages.Add(programImage);
        }));

        await _dbContext.CompaniesImages.AddRangeAsync(generatedImages);
        await _dbContext.SaveChangesAsync();

        return generatedImages;
    }

    public async Task<List<CompanyImage>> GetAllCompanyImagesAsync()
    {
        return await _dbContext.CompaniesImages.ToListAsync();
    }

    public async Task<CompanyImage> DeleteCompanyImageAsync(int id)
    {
        var image = await _dbContext.CompaniesImages.FindAsync(id);
        if (image is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Company Image with ID : {id}");
        }

        _dbContext.CompaniesImages.Remove(image);
        await _dbContext.SaveChangesAsync();

        return image;
    }
}