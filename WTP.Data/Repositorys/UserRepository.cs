using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using WTP.Domain.Dtos.Requests;
using WTP.Domain.Entities;
using WTP.Domain.Entities.Auth;

namespace WTP.Data.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IEmailPassword _mail;

        //private readonly MailSettings _mailSettings;

        public UserRepository(IMapper mapper,
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IEmailPassword mail)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _mail = mail;
            //_mailSettings = mailSettings;
        }

        public async Task AddManager(UserRegistrationDto user, string id)
        {
            Manager manager = new Manager()
            {
                Email = user.Email,
                Name = user.UserName,
                UserId = id
            };

            await _context.AddAsync(manager);
            await _context.SaveChangesAsync();
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        public string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<bool> SendEmailPasswordReset(ForgotPassword model, string origin, string token)
        {
            var user = await _userManager.FindByEmailAsync(model.email);
            if (token != null)
            {
                user.ResetToken = token;
                user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);
                await _userManager.UpdateAsync(user);
            }
            var link = $"{origin}/api/Auth/NewPassword?token={token}&email={user.Email}";
            bool sendEmail = _mail.SendEmailPasswordReset(model, link);
            return sendEmail;
        }

        public async Task<bool> ResetPassword(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null &&
                user.RefreshTokens.ToString() != model.Token &&
                 user.ResetTokenExpires < DateTime.UtcNow)
            {
                return false;
            }
            else
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (resetPassResult.Succeeded)
                {
                    user.PasswordReset = DateTime.UtcNow;
                    user.ResetToken = null;
                    user.ResetTokenExpires = null;
                    await _userManager.UpdateAsync(user);

                    return true;
                }
            }
            return false;
        } 
    }
}