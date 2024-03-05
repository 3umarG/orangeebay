using Orange_Bay.Models.Programs;
using Orange.EF.Repositories.Base;

namespace Orange.EF.Repositories.Impl;

public class ProgramsImagesRepository : BaseImagesRepository<ProgramImage>
{
    public ProgramsImagesRepository(ApplicationDbContext context) : base(context)
    {
    }
}