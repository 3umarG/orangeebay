namespace Orange_Bay.DTOs.Profile;

public class ProfileResponseDto
{
    public int Id { get; set; }
    public int UserTypeId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }

    public string? UserName { get; set; }
    public string? Phone { get; set; }
    public string? PhotoUrl { get; set; }
}