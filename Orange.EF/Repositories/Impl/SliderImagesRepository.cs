using Orange_Bay.Interfaces.Repositories;
using Orange_Bay.Models.SliderImage;
using Orange.EF.Repositories.Base;

namespace Orange.EF.Repositories.Impl;

public class SliderImagesRepository : BaseImagesRepository<SliderImage>
{
    public SliderImagesRepository(ApplicationDbContext context) : base(context)
    {
    }
}