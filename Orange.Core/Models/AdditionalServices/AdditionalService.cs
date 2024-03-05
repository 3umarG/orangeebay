namespace Orange_Bay.Models.AdditionalServices;

public class AdditionalService
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<AdditionalServicePrice> AdditionalServicePrices { get; set; } =
        new HashSet<AdditionalServicePrice>();
}