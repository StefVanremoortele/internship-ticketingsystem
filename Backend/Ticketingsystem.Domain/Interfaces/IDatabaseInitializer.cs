using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ticketingsystem.Domain.Interfaces
{
    public interface IDatabaseInitializer 
    {
        void Seed();
        Task SeedAsync();

    }
}
