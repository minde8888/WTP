using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Data.Repositorys
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ManagerRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddItem(Manager manager)
        {
            _context.Add(manager);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(Guid Id)
        {
            var manager = _context.Manager.Include(x => x.Address).Single(x => x.Id == Id);
            _context.Manager.Remove(manager);
            _context.Address.Remove(manager.Address);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Manager>> GetItemIdAsync(Guid Id)
        {
            return await _context.Manager.Include(maneger => maneger.Address).Where(x => x.Id == Id).ToListAsync(); ;
        }

        public async Task<List<ManagerDto>> GetItemAsync(string ImageSrc)
        {
            var manager = await _context.Manager.Select(x => new Manager()
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
            var manegerDto = _mapper.Map<List<ManagerDto>>(manager);

            return manegerDto;
        }

        public async Task UpdateItem(Guid Id, Manager manager)
        {
            _context.Entry(manager).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
        public async Task<IEnumerable<Manager>> Search(string name)
        {
            IQueryable<Manager> query = _context.Manager;

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
