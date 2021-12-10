using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Domain.Dtos;
using WTP.Domain.Dtos.UpdateDto;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IEmployeesRepository 
    {
        Task<List<Employee>> GetItemIdAsync(Guid Id);
        public Task AddEmployee(string UserId, EmployeeDto employee);
        public Task UpdateEmployee(UpdateEmployeeDto updateManagerDto);
    }
}
