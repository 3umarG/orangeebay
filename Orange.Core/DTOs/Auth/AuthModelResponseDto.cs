using Orange_Bay.DTOs.Profile;

namespace Orange_Bay.DTOs.Auth
{
    public class AuthModelResponseDto : ProfileResponseDto
    {
        public bool IsAuthed { get; set; }
        public string? Token { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}