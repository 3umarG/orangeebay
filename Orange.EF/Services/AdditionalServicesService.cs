using Microsoft.EntityFrameworkCore;
using Orange_Bay.DTOs.AdditionalServices;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;

namespace Orange.EF.Services;

public class AdditionalServicesService : IAdditionalServicesService
{
    private readonly ApplicationDbContext _dbContext;

    public AdditionalServicesService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<AdditionalServiceResponseDto>> GetAllAdditionalServicesByUserTypeIdAsync(int userTypeId)
    {
        if (!await _dbContext.UserTypes.AnyAsync(userType => userType.Id == userTypeId))
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User Type with id : {userTypeId}");
        }

        var services = _dbContext.AdditionalServices
            .Include(service => service.AdditionalServicePrices)
            .Where(service => service.AdditionalServicePrices.Any(price => price.UserTypeId == userTypeId))
            .AsEnumerable()
            .Select(service =>
            {
                var pricePerChild = service.AdditionalServicePrices
                    .FirstOrDefault(price => price.UserTypeId == userTypeId)?.PricePerChild;
                
                var pricePerAdult = service.AdditionalServicePrices
                    .FirstOrDefault(price => price.UserTypeId == userTypeId)?.PricePerAdult;

                return new AdditionalServiceResponseDto(
                    service.Id,
                    service.Name,
                    service.Description,
                    (double)pricePerChild!,
                    (double)pricePerAdult!
                );
            }).ToList();


        return services;
    }
}