using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
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

        public async Task AddEmployee(Manager manager, string  UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            manager.UserId = user.Id;
            await _context.AddAsync(manager);
            await _context.SaveChangesAsync();   
        }
    }
}
