using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos.Requests;
using WTP.Domain.Entities;
using WTP.Domain.Entities.Auth;

namespace WTP.Data.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(UserRegistrationDto user)
        {
            if (user.Roles == "Manager")
            {
                Manager manager = new()
                {
                    UserId = user.UserId,
                    Name = user.UserName,
                    Surname = user.Surname,
                    Email = user.Email,
                    Role = user.Roles,
                    PhoneNumber = user.PhoneNumber
                };

                await _context.AddAsync(manager);
                await _context.SaveChangesAsync();
            }
            if (user.Roles == "Employee")
            {
                Employee employee = new()
                {
                    UserId = user.UserId,
                    Name = user.UserName,
                    Surname = user.Surname,  
                    Email = user.Email,        
                    Role = user.Roles,
                    PhoneNumber = user.PhoneNumber,
                    ManagerId = user.ManagerId
                };

                await _context.AddAsync(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> removeRefreshToken(string rawUserId)
        {
            IEnumerable<RefreshToken> refreshTokens = await _context.RefreshToken
           .Where(t => t.UserId.ToString() == rawUserId)
           .ToListAsync();

            if (refreshTokens == null)
            {
                throw new Exception();
            }
            else
            {
                _context.RefreshToken.RemoveRange(refreshTokens);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}