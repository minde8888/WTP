using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;
using WTP.Domain.Entities.Auth;

namespace WTP.Data.Repositorys
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BaseRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddItemAsync(T t)
        {
            _context.Add(t);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(Guid Id)
        {
            var t = await _context.Set<T>().FindAsync(Id);
            var address = await _context.Address.FindAsync(Id);

            _context.Set<T>().Remove(t);
            _context.Address.Remove(address);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> Search(string name)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name)
                            || e.Surname.Contains(name));
            }
            return await query.ToListAsync();
        }
    }
}