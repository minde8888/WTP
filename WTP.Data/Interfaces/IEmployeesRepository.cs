using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IEmployeesRepository 
    {
        public Task addEmployee(string UserId, Employee employee);
    }
}
