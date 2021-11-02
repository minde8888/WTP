using System.ComponentModel.DataAnnotations;

namespace WTP.Domain.Entities.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(7)]
        public string Password { get; set; }
    }
}