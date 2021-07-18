using Microsoft.AspNetCore.Authorization;
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
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntyti
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddItem(T t)
        {
            await _context.AddAsync(t);
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

        public async Task<List<T>> GetItemIdAsync(Guid Id)
        {
            return await _context.Set<T>().Include(t => t.Address).Where(x => x.Id == Id).ToListAsync(); 
        }
        [Authorize(Roles = "Manager, Administrator")]
        public async Task<List<T>> GetItemAsync(string ImageSrc)
        {
  
               var items =  await _context.Set<T>().Include(t => t.Address)
             //.Select(x => new T()  //??????????????????????????????
             //{
             //    Id = x.Id,
             //    Name = x.Name,
             //    Surname = x.Surname,
             //    Occupation = x.Occupation,
             //    Email = x.Email,
             //    ImageName = x.ImageName,
             //    ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, x.ImageName),
             //    Address = x.Address
             //})
             .ToListAsync();
                foreach (var item in items)
                {
                    item.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, item.ImageName);
                }
                return items;       
            
        }
        [Authorize(Roles = "Manager, Administrator")]
        public async Task UpdateItem(Guid Id, T t)
        {
            _context.Entry(t).State = EntityState.Modified;
            _context.Entry(t.Address).State = EntityState.Modified;
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
        public void DeleteImage(string imagePath)
        {
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
