using Orange_Bay.DTOs.AdditionalServices;

namespace Orange_Bay.Interfaces.Services;

public interface IAdditionalServicesService
{
    Task<List<AdditionalServiceResponseDto>> GetAllAdditionalServicesByUserTypeIdAsync(int userTypeId);
}