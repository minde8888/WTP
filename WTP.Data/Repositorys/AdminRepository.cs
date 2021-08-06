using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;

namespace WTP.Data.Repositorys
{
    public class AdminRepository: IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Manager>> GetManagerAsync(string ImageSrc)
        {
            var items = await _context.Manager.Include(manager => manager.Address).ThenInclude(manager => manager.Employee).ThenInclude(manager => manager.Posts)
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

        public async Task<List<Employee>> GetEmployeeAsync(string ImageSrc)
        {
            var items = await _context.Employee.Include(employee => employee.Address)
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

        public async Task AddItem(Manager manager)
        {
            await _context.AddAsync(manager);
            await _context.SaveChangesAsync();
        }
    }
}
