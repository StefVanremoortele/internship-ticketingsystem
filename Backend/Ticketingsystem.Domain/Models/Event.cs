using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ticketingsystem.Domain.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public string Description { get; set; } = "No Description";

        [Required]
        public DateTime Start { get; set; } 

        [Required]
        public DateTime End { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
}
