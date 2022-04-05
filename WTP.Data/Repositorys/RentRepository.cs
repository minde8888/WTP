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
    public class RentRepository : IRentRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RentRepository(IMapper maper, AppDbContext context)
        {
            _mapper = maper;
            _context = context;
        }

        public async Task<List<Rent>> GetRentIdAsync(Guid rentId)
        {
            return await _context.Rent.
                Include(p => p.Project).
                Where(x => x.RentId == rentId).ToListAsync();
        }
        public async Task AddRentToolAsync(RentDto rent)
        {
                Rent rentToSave = _mapper.Map<Rent>(rent);
                _context.Add(rentToSave);
                await _context.SaveChangesAsync();         
        }
        public async Task<List<RentDto>> GetAllAsync(string ImageSrc)
        {
            var items = await _context.Rent.
                Include(p => p.Project)
                .ToListAsync();

            var i = _mapper.Map<List<RentDto>>(items);

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

        public async Task<Rent> UpdateRent(RentDto rentDto)
        {
            var rent = _context.Rent.
                Include(p => p.Project).
                Where(r => r.RentId == rentDto.RentId).FirstOrDefault();

            rent.Title = rentDto.Title;
            rent.DateUpdated = DateTime.UtcNow;

            if (rentDto.ImageName != null)
            {
                rent.ImageName = rentDto.ImageName;
            }

            _context.Entry(rent).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return rent;
        }

        public async Task RemoveRetedToolAsync(string Id)
        {
            var rented = _context.Rent.Where(x => x.RentId == new Guid(Id)).FirstOrDefault();
            rented.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
