using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
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

        public ManagerRepository(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
  
        public async Task<List<Manager>> GetItemAsync(string ImageSrc)
        {
            var items = await _context.Manager.Include(manager => manager.Address).Include(employee => employee.Employees)
                .ToListAsync();

            var i = _mapper.Map<List<ManagerDto>>(items);

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
