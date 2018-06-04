using System;
using System.Collections.Generic;
using Ticketingsystem.DTO.Tickets;

namespace Ticketingsystem.DTO.Events
{
    public class Event
    {
        public int EventId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

    }
}
