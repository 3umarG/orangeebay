using Orange_Bay.DTOs.ContactUs;
using Orange_Bay.Models.ContactUs;

namespace Orange_Bay.Interfaces.Services;

public interface IContactUsService
{
    Task<ContactUsMessage> AddContactUsMessageAsync(ContactUsMessageRequestDto dto);
    Task<IEnumerable<ContactUsMessage>> GetContactUsMessagesAsync(DateTime? date);
}