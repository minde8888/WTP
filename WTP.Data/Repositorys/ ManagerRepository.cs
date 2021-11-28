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
  
        public async Task<List<ManagerDto>> GetItemAsync(string ImageSrc)
        {
            var items = await _context.Manager.Include(manager => manager.Address).Include(employee => employee.Employees)
                .ToListAsync();

            var i = _mapper.Map<List<ManagerDto>>(items);

            if (items != null)
            {
                foreach (var item in i)
                {
                    item.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, item.ImageName);
                }
                return i;
            }
            return null;
        }

        public async Task AddManager(Manager manager, string  UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            manager.UserId =  new Guid(user.Id.ToString());
            await _context.AddAsync(manager);
            await _context.SaveChangesAsync();   
        }

        public async Task UpdateManager(UpdateManagerDto updateManagerDto)
        {
            var manager = _context.Manager.FirstOrDefault(m => m.Id == updateManagerDto.Id);

            manager.Name = updateManagerDto.Name;
            manager.Surname = updateManagerDto.Surname;
            manager.Occupation = updateManagerDto.Occupation;
            manager.DateUpdated = updateManagerDto.DateUpdated;
            if (updateManagerDto.ImageName != null)
            {
                manager.ImageName = updateManagerDto.ImageName;
            }
            
            _context.Entry(manager).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
