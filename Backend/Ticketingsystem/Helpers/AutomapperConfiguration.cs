using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Helpers
{
    public class AutomapperConfiguration
    {
        public static void ConfigureProfiles()
        {
            // Model mapping
            Mapper.Initialize(cfg =>
            {
                // Models to DTO
                cfg.CreateMap<Event, DTO.Events.Event>();
                cfg.CreateMap<Ticket, DTO.Tickets.Ticket>()
                        .ForMember(td => td.TicketCategory,
                            opt => opt.MapFrom(t => t.TicketCategory));

                cfg.CreateMap<Ticket, DTO.Tickets.TicketForCreation>();
                cfg.CreateMap<User, DTO.Users.User>();
                cfg.CreateMap<TicketHistory, DTO.TicketHistory.TicketHistory>();
                cfg.CreateMap<TicketHistory, DTO.TicketHistory.TicketHistoryForCreation>();
                cfg.CreateMap<Order, DTO.Orders.OrderForUpdate>();
                cfg.CreateMap<Order, DTO.Orders.Order>();
                cfg.CreateMap<Order, DTO.Carts.Cart>()
                        .ForMember(td => td.Tickets,
                            opt => opt.MapFrom(t => t.Tickets)); ;
                cfg.CreateMap<TicketCategory, DTO.TicketCategories.TicketCategory>();
                cfg.CreateMap<Stock, DTO.Stock.Stock>();

                // DTO to Models
                cfg.CreateMap<DTO.Events.EventForCreation, Event>();
                cfg.CreateMap<DTO.Events.EventForUpdate, Event>();
                cfg.CreateMap<DTO.Tickets.Ticket, Ticket>();
                cfg.CreateMap<DTO.Tickets.TicketForUpdate, Ticket>();
                cfg.CreateMap<DTO.Tickets.TicketForCreation, Ticket>();
                cfg.CreateMap<DTO.TicketHistory.TicketHistoryForCreation, TicketHistory>();
                cfg.CreateMap<DTO.TicketHistory.TicketHistory, TicketHistory>();
                cfg.CreateMap<DTO.Orders.OrderForUpdate, Order>();
                cfg.CreateMap<DTO.Orders.OrderForCreation, Order>();
                cfg.CreateMap<DTO.TicketCategories.TicketCategory, TicketCategory>();
                cfg.CreateMap<DTO.Stock.Stock, Stock>();

            });
        }
    }
}
