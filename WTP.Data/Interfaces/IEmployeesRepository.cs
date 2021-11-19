using System.Threading.Tasks;
using WTP.Domain.Dtos;


namespace WTP.Data.Interfaces
{
    public interface IEmployeesRepository 
    {
        public Task AddEmployee(string UserId, EmployeeDto employee);
    }
}
