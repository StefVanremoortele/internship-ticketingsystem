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

            string[] eventNames = new String[] { "The Color Purple", "Glasgow International Comedy Festival", "Vancouver Fashion Week", "DC Environmental Film Festival", "MaerzMusik", "Cape Town Jazz Festival", "Head of the River Race", "Byron Bay Bluesfest", "Rouketopolemos", "Snowbombing", "Toronto Food and Drink Market", "Coachella", "Paris Marathon", "Songkran Water Festival", "Affordable Art Fair", "Sandfest", "Beltane Fire Festival"};
            string[] descriptions = new string[] {
                "Get ready to witness one of the most remarkable musicals of recent times – Marsha Norman’s unlikely onstage adaption of Alice Walker’s 1982 novel “The Color Purple,” telling the sometimes uplifting, often harrowing tale of Celie, an African-American woman in the mid-20th century American South. The winner of multiple awards, the newest incarnation of the musical is a bare-bones version of the original that gets right into the heart of the story. Don’t miss out on this truly unique work of musical theatre – select your show from the list below and book your tickets early to guarantee a good seat.",
                "Get ready to laugh it up at a comedy gathering that organisers call “the largest event of its kind in Europe”, as household names and emerging acts take to the stand-up stage across Scotland’s most cosmopolitan city.",
                "The movers and shakers of Canadian fashion hold their own at this increasingly high profile event. Expect a blizzard of flash photography at glamorous social gatherings and runway shows, tempered by a hearty helping of Canadian hospitality.",
                "At once illuminating and sombre, this no-nonsense film festival delves into the impact of modern human society on the planet that sustains it. Screenings are held at venues around the city, touching on a variety of genres (animation, documentary, narrative) and topics (large-scale agriculture, atomic energy, exploratory adventures, nature films). Feast on a selection of past films available for free streaming at the festival homepage before the main event.",
                "The goal is certainly ambitious: to combine vastly different contemporary genres, from chamber music to media art, into an exploration of what defines music today. But for 13 years, MaerzMusik has managed to do just that through a programme of openness and creativity, brought to life by artists both mainstream and fringe.",
                "Jazz is as much at home in Cape Town as it is in New Orleans. And every year, the city celebrates its rich jazz culture with a soulful festival that attracts major names from near and far. This year, international chart-toppers Carmen Lundy and Tasha’s World join South Africa’s smoothest voices, from Matthew Gold to Jonas Gwangwa, for two days of concerts, parties, master classes and some general groovin’.",
                "Every year, more than 400 crews take to the Thames for a bout of healthy competition to kick off the national rowing season, in a tradition that has held strong since its inauguration in 1926. Spectators gather early to nab a clear viewpoint on the river banks, often with a bottle of Pimms in hand.",
                "Acts as eminent and eclectic as Lenny Kravitz, Hozier, Counting Crows and the Zac Brown Band will all take the stage at this laid-back ode to generations of music. Though it’s no longer blues bash it was 25 years ago, Bluesfest has established itself among the likes of Glastonbury as one of the world’s top music festivals – and one of its most eco-friendly.",
                "Forget dyed eggs and candy baskets, these two churches on the Greek island of Chios really know how to celebrate Easter… by shooting thousands of rockets at each other. Located on two hilltops 400 metres apart, the rivalry between Angios Marcos and Panaghia Ereithiani goes back centuries, and though no one really knows what started this peculiar tradition (some say the rockets were used to mask the sound of forbidden church services during the Ottoman occupation of Greece), it’s certainly one of the more unique ways to observe the holiday.",
                "Is it a snow sports event, a music festival or an excuse to cosy up at an Austrian spa? Yep – it’s all three. Spend the days carving up 500 kilometres of world-class slopes and terrain park, then dance the night away to famous acts (think: Fatboy Slim, Skrillex) at open-air concerts or in the Arctic Disco, a pop-up nightclub in an igloo. When you need a break, retreat to the serenity of a sauna session at one of Mayrhofen’s celebrated spas.",
                "Food trucks, culinary master classes, health food showcases, a local farmers market – this foodie festival has it all.",
                "Drawing huge names and emerging artists across the musical spectrum, Coachella is a notch every festival junky needs in their belt. Go for the bands and DJs, stay for the outrageous art installations, dance-offs, beautiful people and anything-goes atmosphere.",
                "Lace up those running shoes, it’s time to pound the pavement in one of the world’s most beautiful cities. Take in the most iconic Parisian sites – from the Champs-Élysées to the Louvre, Place de la Concorde, the Seine and the Eiffel tower – while testing your endurance along with 37,000 fellow athletes and even more spectators.",
                "Mid-April marks the coming of a new year in Thailand, but you won’t see much fireworks and Champagne here. Instead, the country takes the combination of national holiday and hot weather as an excuse to erupt into a massive water fight – the stuff of every kid’s dreams. Old and young, friends or strangers, everyone is fair game in what might be the world’s biggest splash fest.",
                "Challenging the notion that art ownership is the prerogative of the privileged class, the Affordable Art Fair organisation hosts events around the world aimed at bringing contemporary art into the home of any enthusiast. Singapore’s edition will be held in April, boasting thousands of reasonably priced prints, paintings, sculptures and photography from big names and young, up-and-coming artists.",
                "Everything really is bigger in Texas – even the sand castles. A general “go big or go home” attitude dominates this three-day competition, drawing master sand sculptors from across the globe hoping to qualify for the world sand sculpting championship. As the pros set to work, spectators can enjoy music, food and entertainment, not to mention amateur competitions and sand sculpting workshops.",
                "An ancient Celtic festival welcoming the arrival of warmer weather is re-enacted each year, featuring a theatrical procession and grand bonfire lighting on one of Edinburgh’s main hills. Watch the pageantry unfold, then see if you can resist the temptation to frolic around the bonfire with other revellers."
            };
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
                            Description = descriptions[i],
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
