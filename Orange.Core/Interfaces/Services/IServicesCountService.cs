using Orange_Bay.DTOs.ServicesCount;

namespace Orange_Bay.Interfaces.Services;

public interface IServicesCountService
{
    Task<ServicesCountResponseDto> GetAllServicesItemsCount();
}