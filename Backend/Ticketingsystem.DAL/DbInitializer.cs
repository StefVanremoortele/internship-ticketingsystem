using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticketingsystem.DAL.Helpers;
using Ticketingsystem.DAL.Utilities;
using Ticketingsystem.Domain.Interfaces;
using Ticketingsystem.Domain.Models;
using Ticketingsystem.Logging;

namespace Ticketingsystem.DAL
{
    public class DbInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;

        public DbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();

                if (!_context.Events.Any())
                {
                    // Adding some events
                    var mockEvents = MockDataFactory.CreateEvent();
                    _context.Events.AddRange(mockEvents);

                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                WebHelper.LogWebError("Tickets", Constants.General.ApiName, ex, null);
            }        
        }
        

        public void Seed()
        {
            try
            {
                _context.Database.EnsureCreatedAsync();

                if (!_context.Events.Any())
                {
                    // Adding some events
                    var mockEvents = MockDataFactory.CreateEvent();
                    _context.Events.AddRange(mockEvents);

                }
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                WebHelper.LogWebError("Tickets", Constants.General.ApiName, ex, null);
            }
            
        }
    }
}




