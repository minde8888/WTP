using Microsoft.AspNetCore.Identity;
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

        public async Task addEmployee(string UserId, Employee employee)
        {
            Manager manager = _context.Manager.FirstOrDefault(u => u.UserId == UserId);
            employee.UserId = UserId;
            employee.ManagerId = manager.Id;
            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        //public async Task<IActionResult> AddItem(Manager manager)
        //{
        //    await _context.AddAsync(new Manager()
        //    {
        //        Id = manager.Id,
        //        Name = manager.Name,
        //        Surname = manager.Surname,
        //        Occupation = manager.Occupation,
        //        Email = manager.Email,
        //        ImageName = manager.ImageName,
        //        Address = (new Address()
        //        {
        //            Id = manager.Address.Id,
        //            IsActive = manager.Address.IsActive,
        //            Street = manager.Address.Street,
        //            City = manager.Address.City,
        //            Phone = manager.Address.Phone,
        //            Zip = manager.Address.Zip,
        //            Country = manager.Address.Country,
        //            ManagerId = manager.Address.ManagerId
        //        })
        //    });
        //    await _context.SaveChangesAsync();
        //    return new NoContentResult();
        //}

        //public async Task<List<Manager>> GetItemAsync(string ImageSrc)
        //{
        //    if (_context != null)
        //    {
        //        return await _context.Manager.Include(maneger => maneger.Address)
        //     .Select(x => new Manager()
        //     {
        //         Id = x.Id,
        //         Name = x.Name,
        //         Surname = x.Surname,
        //         Occupation = x.Occupation,
        //         Email = x.Email,
        //         ImageName = x.ImageName,
        //         ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, x.ImageName),
        //         Address = x.Address
        //     })
        //     .ToListAsync();
        //    }
        //    return null;
        //}
    }
}