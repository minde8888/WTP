using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;

namespace WTP.Data.Repositorys
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IMapper _mapper;

        public EmployeesRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            //_mapper = mapper;
        }

        public async Task AddEmployee(string UserId, Employee employee)
        {          
            var user = await _userManager.FindByIdAsync(UserId);
            
            if (user != null)
            {
                employee.UserId = UserId;
                employee.ManagerId = new Guid(user.ManagerId.ToString()); 
                await _context.AddAsync(employee);
                await _context.SaveChangesAsync();//kodel nesaugo i db ______??????????????????
            }
        } 
    }
}