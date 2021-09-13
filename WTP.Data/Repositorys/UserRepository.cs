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

        public async Task AddUser(UserRegistrationDto user, string id)
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

        public async Task<bool> removeRefreshToken(string rawUserId)
        {
            IEnumerable<RefreshToken> refreshTokens = await _context.RefreshToken
           .Where(t => t.UserId == rawUserId)
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