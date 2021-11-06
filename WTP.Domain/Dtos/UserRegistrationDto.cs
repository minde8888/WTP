using System;
using System.ComponentModel.DataAnnotations;

namespace WTP.Domain.Dtos.Requests
{
    public class UserRegistrationDto
    {
        public Guid UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Roles { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
        public Guid? ManagerId { get; set; }
    }
}