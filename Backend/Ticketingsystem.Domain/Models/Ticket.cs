
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ticketingsystem.Domain.Models
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

        [Required]
        public DateTime LastModified { get; set; } = DateTime.Now.ToUniversalTime();

        public TicketStatus TicketStatus { get; set; } = TicketStatus.AVAILABLE;

        public int EventId { get; set; }

        public virtual Event Event { get; set; }


        public int TicketCategoryId { get; set; }

        public TicketCategory TicketCategory { get; set; }

        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }

        public ICollection<TicketHistory> TicketHistory { get; set; } = new List<TicketHistory>();

    }
}
