using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserRepository(
            IMapper mapper,
            AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task AddUser(UserRegistrationDto user)
        {
            if (user.Roles == "Manager")
            {
                Manager manager = _mapper.Map<Manager>(user);

                await _context.AddAsync(manager);
                await _context.SaveChangesAsync();
            }
            if (user.Roles == "Employee")
            {
                Employee employee = _mapper.Map<Employee>(user);

                await _context.AddAsync(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> RemoveRefreshToken(string rawUserId)
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