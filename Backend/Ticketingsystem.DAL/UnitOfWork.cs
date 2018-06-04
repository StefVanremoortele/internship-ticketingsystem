using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ticketingsystem.DAL.Repository;
using Ticketingsystem.Domain.Interfaces;
using Ticketingsystem.Domain.Interfaces.Repository;
using Ticketingsystem.Logging;

namespace Ticketingsystem.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        private IUserRepository _users;
        private IEventRepository _events;
        private IOrderRepository _orders;
        private ITicketRepository _tickets;
        private ITicketCategoryRepository _ticketCategory;
        private ITicketHistoryRepository _ticketHistory;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }


        public IUserRepository Users
        {
            get
            {
                if (_users == null)
                    _users = new UserRepository(_context);

                return _users;
            }
        }

        public IEventRepository Events
        {
            get
            {
                if (_events == null)
                    _events = new EventRepository(_context);

                return _events;
            }
        }

        public IOrderRepository Orders
        {
            get
            {
                if (_orders == null)
                    _orders = new OrderRepository(_context);

                return _orders;
            }
        }

        public ITicketRepository Tickets
        {
            get
            {
                if (_tickets == null)
                    _tickets = new TicketRepository(_context);

                return _tickets;
            }
        }

        public ITicketCategoryRepository TicketCategory
        {
            get
            {
                if (_ticketCategory == null)
                    _ticketCategory = new TicketCategoryRepository(_context);

                return _ticketCategory;
            }
        }
        public ITicketHistoryRepository TicketHistory
        {
            get
            {
                if (_ticketHistory == null)
                    _ticketHistory = new TicketHistoryRepository(_context);

                return _ticketHistory;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                return (_context.SaveChanges() >= 0);
            }
            catch (Exception ex)
            {
                WebHelper.LogWebError("Tickets", Constants.General.ApiName, ex, null);
                return false;
            }
        }
        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return (await _context.SaveChangesAsync() >= 0);
            }
            catch (Exception ex)
            {

                WebHelper.LogWebError("Tickets", Constants.General.ApiName, ex, null);
                return false;
            }
        }
    }
}
