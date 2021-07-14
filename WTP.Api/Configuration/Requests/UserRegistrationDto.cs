using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WTP.Domain.Dtos.Requests
{
    public class UserRegistrationDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Roles { get; set; }
    }
}
