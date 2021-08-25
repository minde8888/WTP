using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Domain.Entities.Auth
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }

    }
}
