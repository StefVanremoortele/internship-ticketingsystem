namespace Ticketingsystem.DAL.Core
{
    public interface IJobConfiguration
    {
        string this[string key] { get; }
    }
}
