using System;
using System.Collections.Generic;
using System.Text;
using Ticketingsystem.DTO.Tickets;

namespace Ticketingsystem.DTO.Carts
{
    public class Cart
    {
        public int OrderId { get; set; }
        public string OrderState { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
