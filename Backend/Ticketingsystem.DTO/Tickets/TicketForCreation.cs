using System;
using System.ComponentModel.DataAnnotations;
using Ticketingsystem.DTO.Orders;
using Ticketingsystem.DTO.TicketCategories;

namespace Ticketingsystem.DTO.Tickets
{
    public class TicketForCreation
    {

        public int EventId { get; set; }

        public TicketCategory Category { get; set; }


    }
}
