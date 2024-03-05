using System.Text.Json.Serialization;
using Orange_Bay.Models.Auth;

namespace Orange_Bay.Models.Programs;

public class ProgramPrice
{
    public int Id { get; set; }
    public double PricePerChild { get; set; }
    public double PricePerAdult { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public int ProgramId { get; set; }
    [JsonIgnore] public Program Program { get; set; }

    public int UserTypeId { get; set; }
    [JsonIgnore] public UserType UserType { get; set; }
}