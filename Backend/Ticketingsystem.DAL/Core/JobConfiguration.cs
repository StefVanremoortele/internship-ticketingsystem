using System.Collections.Specialized;

namespace Ticketingsystem.DAL.Core
{
    public class JobConfiguration : IJobConfiguration
    {
        private NameValueCollection _config;

        public JobConfiguration() { }

        public void AddConfig(NameValueCollection config)
        {
            _config = config;
        }

        public string this[string key] => _config[key];
    }
}
