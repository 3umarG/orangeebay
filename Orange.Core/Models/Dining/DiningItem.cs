using System.Text.Json.Serialization;

namespace Orange_Bay.Models.Dining;

public class DiningItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartFrom { get; set; }
    public string EndAt { get; set; }
    public string FoodType { get; set; }
    public string PhotoUrl { get; set; }
    
    public double? Price { get; set; }

    [JsonIgnore] public DiningCategory DiningCategory { get; set; }
    public int DiningCategoryId { get; set; }
}