using Orange_Bay.Models.CompanyImages;
using Orange.EF.Repositories.Base;

namespace Orange.EF.Repositories.Impl;

public class CompaniesImagesRepository : BaseImagesRepository<CompanyImage>
{
    public CompaniesImagesRepository(ApplicationDbContext context) : base(context)
    {
    }
}