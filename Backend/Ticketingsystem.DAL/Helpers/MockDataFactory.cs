using System;
using System.Collections.Generic;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.DAL.Helpers
{
    public static class MockDataFactory
    {

        public static User CreateUser()
        {
            string[] firstNames = new String[] {
                "Journey",
                "Cheyenne",
                "Aracely",
                "Marlene",
                "Mercedes",
                "Alayna",
                "Kayla",
                "Caitlin",
                "Kelly",
                "Rihanna",
                "Kaiya",
                "Lola",
                "Patience",
                "Dana",
                "Jewel",
                "Brynn",
                "Aleah",
                "Charlee",
                "Jaliyah",
                "Carla",
                "Camilla",
                "Dylan",
                "Sherlyn",
                "Josie",
                "Lorelei",
                "Weston",
                "Donald",
                "Messiah",
                "Edgar",
                "Jaidyn",
                "Jax",
                "Antonio",
                "Alan",
                "Quincy",
                "Stanley",
                "Beckett",
                "Griffin",
                "Hunter",
                "Paxton",
                "Nash",
                "Bennett",
                "Taylor",
                "Darrell",
                "Darion",
                "Jake",
                "Vicente",
                "Justus",
                "Silas",
                "Houston",
                "Brent"
            };
            string[] lastNames = new String[] {
                    "Wilson",
                    "Dodson",
                    "Mcintosh",
                    "Webster",
                    "Diaz",
                    "Haas",
                    "Costa",
                    "Knight",
                    "Ware",
                    "Herring",
                    "Sparks",
                    "Franco",
                    "Hamilton",
                    "Marks",
                    "Little",
                    "Shah",
                    "Dunlap",
                    "Nichols",
                    "Ramsey",
                    "Shea",
                    "Berger",
                    "Cannon",
                    "Horn",
                    "Swanson",
                    "King",
                    "Odom",
                    "Singleton",
                    "Berg",
                    "Baldwin",
                    "Farley",
                    "Garcia",
                    "Velasquez",
                    "Cherry",
                    "Gonzalez",
                    "Carney",
                    "Doyle",
                    "Oconnell",
                    "Benson",
                    "Patterson",
                    "Lawrence",
                    "Everett",
                    "Grant",
                    "Adkins",
                    "Fitzpatrick",
                    "Patterson",
                    "Winters",
                    "Christian",
                    "Vaughan",
                    "Winters",
                    "Walter"
            };
            string[] emails = new String[] {
                "matty@outlook.com",
                "munjal@yahoo.com",
                "dkasak@outlook.com",
                "sumdumass@live.com",
                "pgottsch@hotmail.com",
                "miyop@outlook.com",
                "baveja@comcast.net",
                "doche@live.com",
                "mhoffman@mac.com",
                "wainwrig@yahoo.ca",
                "timlinux@yahoo.com",
                "vertigo@outlook.com",
                "parkes@verizon.net",
                "phizntrg@comcast.net",
                "stinson@me.com",
                "pgolle@yahoo.com",
                "martyloo@msn.com",
                "bruck@gmail.com",
                "mavilar@me.com",
                "bbirth@outlook.com",
                "kudra@verizon.net",
                "empathy@outlook.com",
                "fangorn@optonline.net",
                "godeke@hotmail.com",
                "dhwon@icloud.com",
                "codex@mac.com",
                "jimmichie@live.com",
                "kudra@outlook.com",
                "plover@icloud.com",
                "meder@yahoo.com",
                "improv@yahoo.com",
                "mglee@att.net",
                "flavell@icloud.com",
                "josem@icloud.com",
                "wsnyder@verizon.net",
                "calin@msn.com",
                "jgoerzen@icloud.com",
                "sjmuir@outlook.com",
                "wbarker@aol.com",
                "pakaste@optonline.net",
                "syrinx@me.com",
                "scitext@comcast.net",
                "pavel@yahoo.ca",
                "sisyphus@me.com",
                "aracne@att.net",
                "tmccarth@aol.com",
                "jgmyers@comcast.net",
                "amaranth@mac.com",
                "gknauss@yahoo.ca",
                "jeteve@mac.com"
            };
      
            Random rnd = new Random();
            User user = new User
            {
                //FirstName = firstNames[rnd.Next(0,S firstNames.Length)],
                //LastName = lastNames[rnd.Next(0, lastNames.Length)],
                //Email = emails[rnd.Next(0, emails.Length)],
                //Password = "password",
                //UserType = UserType.CUSTOMER
            };

            return user;
        }
        public static List<Event> CreateEvent()
        {
            string[] eventNames = new String[] { "Ace", "Tech", "Fest", "Vantage", "Adastra", "Affinity", "AFOSEC", "TechFest", "ASEEMIT", "Aurora", "Blitzkrieg", "Brainwaves", "BurnOut", "CITRONICS", "CodeFest", "Credenz", "Cynet","DYNAFEST" };
            TicketCategory category1 = new TicketCategory { Price = 20, Type = TicketType.EARLY_BIRD };
            TicketCategory category2 = new TicketCategory { Price = 50, Type = TicketType.PROMO };
            TicketCategory category3 = new TicketCategory { Price = 125, Type = TicketType.GROUP };
            TicketCategory category4 = new TicketCategory { Price = 250, Type = TicketType.VIP };


            List<Event> events = new List<Event>();
            for (int i = 0; i < eventNames.Length; i++)
            {
                List<Ticket> tickets = new List<Ticket>();

                // 4 verschillende soorten tickets
                for (int k = 0; k < 4; k++)
                {
                    List<Ticket> randomTickets = new List<Ticket>();                    

                    // 100 van elke soort
                    for (int j = 0; j < 20; j++)
                    {
                        randomTickets.Add(CreateTicket(category1));
                    }
                    // 100 van elke soort
                    for (int j = 0; j < 50; j++)
                    {
                        randomTickets.Add(CreateTicket(category2));
                    }
                    // 100 van elke soort
                    for (int j = 0; j < 100; j++)
                    {
                        randomTickets.Add(CreateTicket(category3));
                    }
                    // 100 van elke soort
                    for (int j = 0; j < 200; j++)
                    {
                        randomTickets.Add(CreateTicket(category4));
                    }
                    tickets.AddRange(randomTickets);
                }

                events.Add(
                    new Event
                        {
                            Name = eventNames[i],
                            Start = DateTime.Now.ToUniversalTime(),
                            End = DateTime.Now.ToUniversalTime(),
                            Tickets = tickets
                        }
                    );
            }           

            return events;
        }
        
        public static Ticket CreateTicket(TicketCategory category)
        {
            Random rnd = new Random();
            Ticket ticket = new Ticket
            {
                TicketCategory = category
            };

            return ticket;
        }
    }
}
