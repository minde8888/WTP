using AutoMapper;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos.Requests;
using WTP.Domain.Entities;


namespace WTP.Data.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(
            IMapper mapper,
            AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task AddUserAsync(UserRegistrationDto user)
        {
            if (user.Role == "Manager")
            {
                Manager manager = _mapper.Map<Manager>(user);
                _context.Manager.Add(manager);
                Address addres = new();
                addres.ManagerId = manager.Id;
                _context.Address.Add(addres);

                await _context.SaveChangesAsync();
            }
            if (user.Role == "Employee")
            {
                Employee employee = _mapper.Map<Employee>(user);
                _context.Add(employee);
                Address addres = new();
                addres.EmployeeId = employee.Id;
                _context.Add(addres);

                await _context.SaveChangesAsync();
            }
        }
    }
}