using Orange_Bay.Models.Programs;
using Orange.EF.Repositories.Base;

namespace Orange.EF.Repositories.Impl;

public class ProgramsRepository : BaseRepository<Program>
{
    public ProgramsRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}