namespace Orange_Bay.Models.Gallery;

public class GalleryImageType
{
    public int Id { get; set; }
    public string Type { get; set; }

    public virtual ICollection<GalleryImage> GalleryImages { get; set; } = new HashSet<GalleryImage>();
}