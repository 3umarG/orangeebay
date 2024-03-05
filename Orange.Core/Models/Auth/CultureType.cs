namespace Orange_Bay.Models.Auth;

public class CultureType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } = new HashSet<ApplicationUser>();
}