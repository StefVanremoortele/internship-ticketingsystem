using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Ticketingsystem.DAL;
using Ticketingsystem.DAL.Repository;
using Ticketingsystem.DAL.Services;
using Ticketingsystem.Domain.Interfaces;
using Ticketingsystem.Domain.Interfaces.Services;

namespace Ticketingsystem.App_Start
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<QuartzService>().As<IQuartzService>().InstancePerLifetimeScope();
            builder.RegisterType<EventService>().As<IEventService>().InstancePerLifetimeScope();
            builder.RegisterType<DbInitializer>().As<IDatabaseInitializer>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<TicketService>().As<ITicketService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        }
    }
}
