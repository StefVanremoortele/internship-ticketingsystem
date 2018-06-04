namespace Ticketingsystem.DTO.TicketCategories
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
        
        public int Price { get; set; }

        public string Type { get; set; }

    }
}
