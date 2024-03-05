namespace Orange_Bay.DTOs.ContactUs;

public record ContactUsMessageRequestDto(
    string FullName,
    string Email,
    string Phone,
    string Subject,
    string Message
);