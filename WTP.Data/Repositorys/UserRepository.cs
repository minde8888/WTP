using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Realms.Sync.Exceptions;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Helpers;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos.Requests;
using WTP.Domain.Entities;
using WTP.Domain.Entities.Auth;
using WTP.Domain.Entities.Settings;

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

        public async Task<bool> SendEmailPasswordReset(ForgotPassword model, string origin, string token )
        {
            var user = await _userManager.FindByEmailAsync(model.email);
            if (token != null)
            {
                
                user.ResetToken = token;
                await _userManager.UpdateAsync(user);
                _context.SaveChanges();
            }
            var link = $"{origin}/api/Auth/NewPassword?token={token}{user.Id}";
            bool sendEmail = _mail.SendEmailPasswordReset(model, link);
            return sendEmail;

        }

        public async Task  ResetPassword(ResetPasswordRequest model)
        {
            var a = model;
           var b = a;
            //_userManager.FindFir
            //var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            //var account = _context._User. (x =>
            //      x.ResetToken == model.Token &&
            //      x.ResetTokenExpires > DateTime.UtcNow);

            //if (account == null)
            //    throw new AppException("Invalid token");

            //// update password and remove reset token
            //account.PasswordHash = BC.HashPassword(model.Password);
            //account.PasswordReset = DateTime.UtcNow;
            //account.ResetToken = null;
            //account.ResetTokenExpires = null;

            //_context.Accounts.Update(account);
            //_context.SaveChanges();
        }

    }
}