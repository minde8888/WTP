using System;
using System.ComponentModel.DataAnnotations;

namespace WTP.Domain.Dtos.Requests
{
    public class UserRegistrationDto
    {
        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Roles { get; set; }

        public string? ManagerId { get; set; }
    }
}