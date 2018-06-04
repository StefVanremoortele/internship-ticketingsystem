using System;

namespace Ticketingsystem.DTO.TicketHistory
{
    public enum TicketAction
    {
        RESERVED = 0,
        RELEASED = 1,
        SOLD = 2
    }


    public class TicketHistory
    {

        public DateTime Timestamp { get; set; }

        public string TicketAction { get; set; }

        public int TicketId { get; set; }

        public int? OrderId { get; set; }


    }
}
