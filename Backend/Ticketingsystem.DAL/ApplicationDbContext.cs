using Microsoft.EntityFrameworkCore;
using System;
using Ticketingsystem.Domain.Models;


namespace Ticketingsystem.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public string CurrentUserId;

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<TicketCategory> TicketCategory { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Event> Events { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
