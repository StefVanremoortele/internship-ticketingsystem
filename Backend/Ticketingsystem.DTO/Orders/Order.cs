using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ticketingsystem.DTO.Tickets;

namespace Ticketingsystem.DTO.Orders
{
    public enum OrderState
    {
        PENDING = 0,
        EXPIRED = 1,
        CANCELED = 2,
        COMPLETE = 3
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string OrderState { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }

}
