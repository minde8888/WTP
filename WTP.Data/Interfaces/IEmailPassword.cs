using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Domain.Entities.Auth;

namespace WTP.Data.Interfaces
{
    public interface IEmailPassword
    {
        public bool SendEmailPasswordReset(ForgotPassword model, string link);
    }
}
