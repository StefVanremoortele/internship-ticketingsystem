using System;
using System.Collections.Generic;
using System.Text;
using Ticketingsystem.DTO.TicketCategories;

namespace Ticketingsystem.DTO.Stock
{
    public class Stock
    {
        public string TicketStatus { get; set; }
        public string TicketType { get; set; }
        public string EventName { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
    }
}
