using System;

namespace Ticketingsystem.DTO.Events
{
    public class EventForUpdate
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

    }
}
