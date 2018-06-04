using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Ticketingsystem.Core
{
    public class StockHub : Hub
    {
        public Task SendStockUpdateToGroup(string groupName, IEnumerable<DTO.Stock.Stock> newStock)
        {
            return Clients.All.SendAsync("Send", newStock);
        }

        public async Task RegisterOnGroup(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);
        }

        public async Task UnRegisterFromGroup(string groupName)
        {
            await Groups.RemoveAsync(Context.ConnectionId, groupName);
        }

    }
}
