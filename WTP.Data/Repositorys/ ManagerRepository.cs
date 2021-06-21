using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ManagerRepository : IManager
    {
        private readonly AppDbContext _context;

        public ManagerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddItem(Manager manager)
        {
            await _context.AddAsync(manager);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        public async Task<IActionResult> DeleteItem(Guid Id)
        {
            var manager = await _context.Manager.FindAsync(Id);
            var address = await _context.Address.FindAsync(Id);
            if (manager == null && address == null)
                return new NotFoundResult();

            _context.Manager.Remove(manager);
            _context.Address.Remove(address);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<List<Manager>> GetItemIdAsync(Guid Id)
        {
            return await _context.Manager.Include(maneger => maneger.Address).Where(x => x.Id == Id).ToListAsync(); ;
        }

        public async Task<List<Manager>> GetItemAsync(string ImageSrc)
        {
            if (_context != null)
            {
                return await _context.Manager.Include(maneger => maneger.Address)
             .Select(x => new Manager()
             {
                 Id = x.Id,
                 Name = x.Name,
                 Surname = x.Surname,
                 Occupation = x.Occupation,
                 Email = x.Email,
                 ImageName = x.ImageName,
                 ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, x.ImageName),
                 Address = x.Address
             })
             .ToListAsync();
            }
            return null;
        }

        public async Task<IActionResult> UpdateItem(Guid Id, Manager manager)
        {
            _context.Entry(manager).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                var manegeUpdate = await GetItemIdAsync(Id);
                if (manegeUpdate == null)
                    return new NotFoundResult();
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return new NoContentResult();
        }
        public async Task<IEnumerable<Manager>> Search(string name, string surname)
        {
            IQueryable<Manager> query = _context.Manager;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name)
                            || e.Surname.Contains(name));
            }

            if (surname != null)
            {
                query = query.Where(e => e.Surname == surname);
            }

            return await query.ToListAsync();
        }
        public void DeleteImage(string imagePath)
        {
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
