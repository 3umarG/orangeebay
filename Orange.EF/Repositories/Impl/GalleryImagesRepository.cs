using Orange_Bay.Models.Gallery;
using Orange.EF.Repositories.Base;

namespace Orange.EF.Repositories.Impl;

public class GalleryImagesRepository : BaseImagesRepository<GalleryImage>
{
    public GalleryImagesRepository(ApplicationDbContext context) : base(context)
    {
    }
}