using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using WTP.Domain.Dtos.UpdateDto;
using WTP.Domain.Entities;
using WTP.Domain.Entities.Auth;

namespace WTP.Data.Repositorys
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManagerRepository(IMapper mapper, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<Manager>> GetItemIdAsync(Guid Id)
        {
            return await _context.Manager.
                Include(address => address.Address).
                Include(employee => employee.Employees).
                Include(post => post.Posts).
                Where(x => x.Id == Id).ToListAsync();
        }

        public async Task<List<ManagerDto>> GetManagerAsync(string ImageSrc)
        {
            var manager = await _context.Manager.
                Include(manager => manager.Address).
                Include(employee => employee.Employees)
                .ToListAsync();

            var i = _mapper.Map<List<ManagerDto>>(manager);

            if (manager != null)
            {
                foreach (var item in i)
                {
                    item.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, item.ImageName);
                }
                return i;
            }
            return null;
        }

        public async Task AddManagerAsync(Manager manager, string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            manager.UserId = new Guid(user.Id.ToString());
            _context.Add(manager);
            await _context.SaveChangesAsync();
        }

        public async Task<Manager> UpdateManager(RequestManagerDto updateManagerDto)
        {
            var manager = _context.Manager.
                Include(manager => manager.Address).
                Where(m => m.Id == updateManagerDto.Id).FirstOrDefault();

            manager.Name = updateManagerDto.Name;
            manager.Surname = updateManagerDto.Surname;
            manager.Occupation = updateManagerDto.Occupation;
            manager.DateUpdated = DateTime.UtcNow;

            if (updateManagerDto.ImageName != null)
            {
                manager.ImageName = updateManagerDto.ImageName;
            }
            if (manager.Address != null)
            {
                manager.Address.City = updateManagerDto.Address.City;
                manager.Address.Country = updateManagerDto.Address.Country;
                manager.Address.Street = updateManagerDto.Address.Street;
                manager.Address.Zip = updateManagerDto.Address.Zip;
                _context.Entry(manager.Address).State = EntityState.Modified;
            }
            else
            {
                Address address = new()
                {
                    City = updateManagerDto.Address.City,
                    Country = updateManagerDto.Address.Country,
                    Street = updateManagerDto.Address.Street,
                    Zip = updateManagerDto.Address.Zip,
                    ManagerId = updateManagerDto.Id
                };
                _context.Address.Add(address);
            }

            _context.Entry(manager).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return manager;
        }

        public async Task RemoveManagerAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var manager = _context.Manager.Where(x => x.UserId == new Guid(userId)).FirstOrDefault();
            manager.IsDeleted = true;
            user.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}