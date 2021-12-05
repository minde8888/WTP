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
using WTP.Domain.Entities;
using WTP.Domain.Entities.Auth;

namespace WTP.Data.Repositorys
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public EmployeesRepository(AppDbContext context, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<Employee>> GetItemIdAsync(Guid Id)
        {
            return await _context.Employee.
               Include(t => t.Address).
               Include(p => p.Posts).
               Where(x => x.Id == Id).ToListAsync();
        }

        public async Task AddEmployee(string UserId, EmployeeDto employee)
        {          
            var user = await _userManager.FindByIdAsync(UserId);
            
            if (user.Roles == "Manager")
            {
                employee.ManagerId = new Guid(UserId.ToString());

                Employee newEmploy = _mapper.Map<Employee>(employee);

                await _context.AddAsync(newEmploy);
                await _context.SaveChangesAsync();
            }
        } 
    }
}