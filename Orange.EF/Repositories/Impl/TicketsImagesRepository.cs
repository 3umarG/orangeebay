using Orange_Bay.Models.Tickets;
using Orange.EF.Repositories.Base;

namespace Orange.EF.Repositories.Impl;

public class TicketsImagesRepository : BaseImagesRepository<TicketImage>
{
    public TicketsImagesRepository(ApplicationDbContext context) : base(context)
    {
    }
}