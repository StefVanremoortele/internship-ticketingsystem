using System;
using System.Collections.Generic;
using Ticketingsystem.DTO.Tickets;

namespace Ticketingsystem.DTO.Events
{
    public class EventForCreation
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }


    }
}
