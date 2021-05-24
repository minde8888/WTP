using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;

namespace WTP.Data.Repositorys
{
    public class ManagerRepository : IManager
    {
        private readonly AppDbContext _context;
        //private readonly IMapper _mapper;

        public ManagerRepository(AppDbContext context)
        {
            _context = context;
            //_mapper = mapper;
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
            //return await (from p in db.Post
            //              from c in db.Category
            //              where p.CategoryId == c.Id
            //              select new PostViewModel
            //              {
            //                  PostId = p.PostId,
            //                  Title = p.Title,
            //                  Description = p.Description,
            //                  CategoryId = p.CategoryId,
            //                  CategoryName = c.Name,
            //                  CreatedDate = p.CreatedDate
            //              }).ToListAsync();
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
        public async Task Search(string name, string surname)
        {
            _context.Manager.Where(n => n.Name == name);
        }
        public void DeleteImage(string imagePath)
        {
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }

    }
}
