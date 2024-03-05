using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Orange_Bay.DTOs.Auth
{
    public class UserRegisterDto : UserLoginDto
    {
        public string UserName { get; set; }
        [Required] public string FullName { get; set; }

        public string? Phone { get; set; }
        [Required] public int UserTypeId { get; set; }

        public IFormFile? Image { get; set; }
    }
}