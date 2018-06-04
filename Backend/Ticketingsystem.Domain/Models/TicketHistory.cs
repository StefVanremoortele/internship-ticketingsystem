using System;
using System.ComponentModel.DataAnnotations;

namespace Ticketingsystem.Domain.Models
{
    public enum TicketAction
    {
        RESERVED=0,
        RELEASED=1,
        SOLD=2
    }

    public class TicketHistory
    {
        public int TicketHistoryId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now.ToUniversalTime();


        [Required]
        [EnumDataType(typeof(TicketAction))]
        public TicketAction TicketAction { get; set; }

        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
        
    }
}
