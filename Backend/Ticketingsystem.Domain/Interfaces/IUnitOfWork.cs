using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Interfaces.Repository;

namespace Ticketingsystem.Domain.Interfaces
{
    public interface IUnitOfWork
    {

        IUserRepository Users { get; }
        IEventRepository Events { get; }
        IOrderRepository Orders { get; }
        ITicketRepository Tickets { get; }
        ITicketCategoryRepository TicketCategory { get; }
        ITicketHistoryRepository TicketHistory { get; }


        Task<bool> SaveChangesAsync();
        bool SaveChanges();
    }
}
