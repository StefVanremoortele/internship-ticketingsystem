using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ticketingsystem.DTO.TicketCategories;
using Event = Ticketingsystem.DTO.Events.Event;
using Order = Ticketingsystem.DTO.Orders.Order;

namespace Ticketingsystem.DTO.Tickets
{
    public enum TicketStatus
    {
        AVAILABLE = 0,
        RESERVED = 1,
        SOLD = 2
    }

    public class Ticket
    {

        public int TicketId { get; set; }

        public int EventId { get; set; }
        public string EventName { get; set; }

        public string TicketStatus { get; set; }

        public TicketCategory TicketCategory { get; set; }

        public DateTime LastModified { get; set; }

    }
}
