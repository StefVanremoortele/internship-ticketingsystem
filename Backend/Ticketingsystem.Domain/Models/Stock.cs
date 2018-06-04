using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketingsystem.Domain.Models
{
    public class Stock
    {
        public TicketStatus TicketStatus { get; set; }
        public TicketType TicketType { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
    }
}
