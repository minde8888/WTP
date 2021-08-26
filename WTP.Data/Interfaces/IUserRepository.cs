using System;
using System.Threading.Tasks;
using WTP.Domain.Dtos.Requests;
using WTP.Domain.Entities.Auth;

namespace WTP.Data.Interfaces
{
    public interface IUserRepository
    {
        public DateTime UnixTimeStampToDateTime(double utcExpiryDate);

        public string RandomString(int length);

        public Task AddManager(UserRegistrationDto user, string id);

        public Task<bool> SendEmailPasswordReset(ForgotPassword model, string token, string origin);

        public Task<bool> ResetPassword(ResetPasswordRequest model);
    }
}