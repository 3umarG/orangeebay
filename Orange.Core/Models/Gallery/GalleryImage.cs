using Newtonsoft.Json;

namespace Orange_Bay.Models.Gallery;

public class GalleryImage
{
    public int Id { get; set; }
    public string PhotoUrl { get; set; }

    public GalleryImageType Type { get; set; }
    public int TypeId { get; set; }
    public string ImageType { get; set; }
}