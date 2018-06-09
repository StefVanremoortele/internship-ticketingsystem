namespace Ticketingsystem.DTO.Users
{
    public enum UserType
    {
        CUSTOMER = 0,
        ADMINISTRATOR = 1
    }

    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public string UserType { get; set; }
    }
}
