using Orange_Bay.Models.Auth;
using Orange_Bay.Models.Programs;

namespace Orange_Bay.Models.Wishlist;

public class ProgramWishlist
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int ProgramId { get; set; }
    public Program Program { get; set; }
}