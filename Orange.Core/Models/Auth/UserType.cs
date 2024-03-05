using System.Text.Json.Serialization;
using Orange_Bay.Models.Programs;

namespace Orange_Bay.Models.Auth;

public class UserType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [JsonIgnore]
    public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } = new HashSet<ApplicationUser>();

    [JsonIgnore] public virtual ICollection<ProgramPrice> ProgramPrices { get; set; } = new HashSet<ProgramPrice>();
}