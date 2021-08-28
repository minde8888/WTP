using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Repositorys;
using WTP.Domain.Entities.Auth;
using WTP.Services.Services.Dtos;

namespace WTP.Services.Services
{
    public class AuthService
    {
        private AppDbContext _context;
        private IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserRepository _userRepository;

        public AuthService(IMapper mapper, 
            AppDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<EmployeeInformationDto>> GetUserInfo(ApplicationUser user)
        {
            var role = await _userManager.GetRolesAsync(user);

            foreach (var item in role)
            {
                switch (item)
                {
                    case "Manager":
                        var manager = await _context.Manager
                        .Include(employee => employee.Employees)
                        .Include(post => post.Posts)
                        .Where(u => u.UserId == user.Id)
                        .ToListAsync();
                        var managerDto = _mapper.Map<List<EmployeeInformationDto>>(manager);
                        return managerDto;

                    case "Employee":
                        var employee = await _context.Employee
                            .Include(post => post.Posts)
                            .Where(u => u.UserId == user.Id)
                            .ToListAsync();
                        var employeeDto = _mapper.Map<List<EmployeeInformationDto>>(employee);
                        return employeeDto;
                    default:
                        throw new Exception();
                }
            }

            throw new Exception();
        }
    }
}