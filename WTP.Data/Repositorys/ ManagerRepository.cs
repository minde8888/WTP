using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;

namespace WTP.Data.Repositorys
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly AppDbContext _context;

        public ManagerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Manager>> GetItemAsync(string ImageSrc)
        {
            var items = await _context.Manager.Include(manager => manager.Address).Include(employee => employee.Employees)
                .ToListAsync();
            if (items != null)
            {
                foreach (var item in items)
                {
                    item.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, item.ImageName);
                }
                return items;
            }
            return null;
        }


    }
}
