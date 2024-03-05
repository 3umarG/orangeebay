using System.Text.Json.Serialization;
using Orange_Bay.Models.Auth;

namespace Orange_Bay.Models.AdditionalServices;

public class AdditionalServicePrice
{
    public int Id { get; set; }
    public double PricePerChild { get; set; }
    public double PricePerAdult { get; set; }

    public int ServiceId { get; set; }
    [JsonIgnore] public AdditionalService Service { get; set; }

    public int UserTypeId { get; set; }
    [JsonIgnore] public UserType UserType { get; set; }
}