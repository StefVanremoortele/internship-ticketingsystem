using System;
using System.ComponentModel.DataAnnotations;

namespace Ticketingsystem.Domain.Models
{
    public enum UserType
    {
        CUSTOMER = 0,
        ADMINISTRATOR = 1
    }

    public class User
    {
        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }

        //[Required]
        public string FirstName { get; set; }
        //[Required]
        public string LastName { get; set; }
        //[Required]
        public string Email { get; set; }

        //[Required]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Boolean IsLoggedIn { get; set; }

        [Required]
        public UserType UserType { get; set; }
    }
}
