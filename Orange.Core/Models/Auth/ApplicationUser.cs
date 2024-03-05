using Microsoft.AspNetCore.Identity;
using Orange_Bay.Models.Programs;

namespace Orange_Bay.Models.Auth
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }

        public int UserTypeId { get; set; }

        public UserType UserType { get; set; }

        public int CultureTypeId { get; set; }

        public CultureType CultureType { get; set; }

        public string? PhotoUrl { get; set; }


        public virtual ICollection<ProgramReview> ProgramReviews { get; set; } = new HashSet<ProgramReview>();
    }
}