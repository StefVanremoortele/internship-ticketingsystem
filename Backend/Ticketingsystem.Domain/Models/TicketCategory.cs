using System.ComponentModel.DataAnnotations;

namespace Ticketingsystem.Domain.Models
{
    public enum TicketType
    {
        EARLY_BIRD = 0,
        PROMO = 1,
        VIP = 2,
        GROUP = 3
    }

    public class TicketCategory
    {
        public int TicketCategoryId { get; set; }

        [Required]
        public int Price { get; set; }

        [EnumDataType(typeof(TicketType))]
        public TicketType Type { get; set; }

    }
}
