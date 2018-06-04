using System;
using System.Collections.Generic;
using System.Text;
using Ticketingsystem.DTO.TicketCategories;

namespace Ticketingsystem.DTO.Tickets
{
    public class BoughtTicket
    {
        public int TicketId { get; set; }

        public int EventId { get; set; }
        public string EventName { get; set; }

        public string TicketStatus { get; set; }

        public TicketCategory TicketCategory { get; set; }
        
    }
}
