using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using Ticketingsystem.Domain.Helpers;

namespace Ticketingsystem.Domain.Models
{
    public enum OrderState
    {
        PENDING=0,
        EXPIRED=1,
        CANCELED=2,
        COMPLETE=3
    }
    public class Order
    {
        public int OrderId { get; set; }


        [EnumDataType(typeof(OrderState))]
        public OrderState OrderState { get; set; } = OrderState.PENDING;


        [Required]
        public DateTime DateOpened { get; set; } = DateTime.Now.ToUniversalTime();
        
        public DateTime? DateClosed { get; set; } 
        
        public string UserId { get; set; }

        public User User { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
}
