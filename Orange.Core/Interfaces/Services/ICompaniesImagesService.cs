using Orange_Bay.DTOs.Programs.Request;
using Orange_Bay.Models.CompanyImages;

namespace Orange_Bay.Interfaces.Services;

public interface ICompaniesImagesService
{
    Task<List<CompanyImage>> AddCompanyImagesAsync(ImagesRequestDto imagesRequestDto);
    Task<List<CompanyImage>> GetAllCompanyImagesAsync();
    Task<CompanyImage> DeleteCompanyImageAsync(int id);
}