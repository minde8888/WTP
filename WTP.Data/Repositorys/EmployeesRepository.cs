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

        public async Task AddEmployee(string UserId, RequestEmployeeDto employee)
        {          
            var user = await _userManager.FindByIdAsync(UserId);
            
            if (user.Roles == "Employee")
            {
                employee.ManagerId = new Guid(UserId.ToString());

                Employee newEmploy = _mapper.Map<Employee>(employee);

                await _context.AddAsync(newEmploy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateEmployee(RequestEmployeeDto updateEmployeeDto)
        {
            var employee = _context.Employee.
                Include(employee => employee.Address).
                Where(m => m.Id == updateEmployeeDto.Id).FirstOrDefault();

            employee.Name = updateEmployeeDto.Name;
            employee.Surname = updateEmployeeDto.Surname;
            employee.Occupation = updateEmployeeDto.Occupation;
            employee.DateUpdated = updateEmployeeDto.DateUpdated;
            if (updateEmployeeDto.ImageName != null)
            {
                employee.ImageName = updateEmployeeDto.ImageName;
            }

            if (employee.Address != null)
            {
                employee.Address.City = updateEmployeeDto.Address.City;
                employee.Address.Country = updateEmployeeDto.Address.Country;
                employee.Address.Street = updateEmployeeDto.Address.Street;
                employee.Address.Zip = updateEmployeeDto.Address.Zip;
                _context.Entry(employee.Address).State = EntityState.Modified;
            }
            else
            {
                Address address = new()
                {
                    City = updateEmployeeDto.Address.City,
                    Country = updateEmployeeDto.Address.Country,
                    Street = updateEmployeeDto.Address.Street,
                    Zip = updateEmployeeDto.Address.Zip,
                    ManagerId = updateEmployeeDto.Id
                };
                _context.Address.Add(address);
            }

            _context.Entry(employee).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
        public async Task RemoveEmployeeAsync(string userId)
        {
            var employee =  _context.Employee.Where(x => x.Id == Guid.Parse(userId)).FirstOrDefault();
            var user = await _userManager.FindByEmailAsync(employee.Email);
            employee.IsDeleted = true;
            user.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}