using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;
using WTP.Domain.Entities.Auth;

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
            
            if (user.Roles == "Manager")
            {
  
                //Manager manager = new Manager();
                //manager = _context.Manager.F;
                employee.ManagerId = new Guid(UserId.ToString());

                //employee.ManagerId = new Guid(Manager.Id.ToString());
                //var a = _context.Manager.Find(UserId);
                //employee.ManagerId = new Guid();
                await _context.AddAsync(employee);
                await _context.SaveChangesAsync();
            }
        } 
    }
}