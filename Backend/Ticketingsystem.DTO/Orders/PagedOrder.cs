using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketingsystem.DTO.Orders
{
    public class PagedOrder
    {
        public IEnumerable<Order> Orders { get; set; }
        public int Total_count { get; set; }
    }
}
