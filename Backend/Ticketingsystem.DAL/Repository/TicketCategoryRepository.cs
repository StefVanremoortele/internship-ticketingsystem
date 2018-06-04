using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Repository;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.DAL.Repository
{
    public class TicketCategoryRepository : Repository<TicketCategory>, ITicketCategoryRepository
    {
        private ApplicationDbContext _ctx => (ApplicationDbContext)_context;

        public TicketCategoryRepository(ApplicationDbContext context) : base(context)
        { }


        public async Task<RepositoryActionResult<IEnumerable<TicketCategory>>> GetAllTicketCategories()
        {
            try
            {
                IEnumerable<TicketCategory> categories = await _ctx.TicketCategory.ToListAsync();

                if (categories == null)
                    return new RepositoryActionResult<IEnumerable<TicketCategory>>(categories, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<TicketCategory>>(categories, RepositoryActionStatus.Ok);
            }
            catch (Exception)
            {
                return new RepositoryActionResult<IEnumerable<TicketCategory>>(null, RepositoryActionStatus.Ok);
            }
        }

    }
}
