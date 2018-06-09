using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketingsystem.DTO.Roles
{

    public enum UserType
    {
        CUSTOMER = 0,
        ADMINISTRATOR = 1
    }

    public class Role
    {
        public string UserType { get; set; }
    }
}
