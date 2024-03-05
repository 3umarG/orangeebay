namespace Orange_Bay.DTOs.Dashboard;

public class DashboardAdditionalServiceRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<AdditionalServicePriceRequestDto> Prices { get; set; }
}

public class AdditionalServicePriceRequestDto
{
    public double PricePerChild { get; set; }
    public double PricePerAdult { get; set; }
    public int UserTypeId { get; set; }
}