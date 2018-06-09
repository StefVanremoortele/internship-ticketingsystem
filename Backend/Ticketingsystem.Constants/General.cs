using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketingsystem.Constants
{
    public static class General
    {
        public const string Authority_URI = "http://localhost:5000";
        public const string MvcClient_URI = "http://localhost:5002";
        public const string AngularClient_URI = "http://localhost:4200";

        //public const string API_URI = "http://localhost:5001";
        public const string API_URI = "http://ticketing-api1.westeurope.cloudapp.azure.com";

        //public const string Authority_URI = "http://identity-provider.eastus.cloudapp.azure.com";
        //public const string API_1_URI = "http://ticketing-api1.westeurope.cloudapp.azure.com";
        //public const string API_2_URI = "http://ticketing-api2.westeurope.cloudapp.azure.com";
        //public const string AngularClient_URI = "https://internship-ticketing-system.azurewebsites.net";

        public const string ApiName = "Ticketingsystem_API";

        public const string ConsoleClientId = "ConsoleClient";
        public const string MvcClientId = "MvcClient";
        public const string AngularClientId = "AngularClient";


        public const string ConsoleClientSecret = "secret";
        public const string MvcClientSecret = "secret";
        public const string AngularClientSecret = "secret";
        public const string ApiSecret = "secret";

        public const string SCOPE_READ = "Ticketingsystem.READ";
        public const string SCOPE_WRITE = "Ticketingsystem.WRITE";
    }
}
