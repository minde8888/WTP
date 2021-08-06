using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Domain.Dtos
{
    public class ApplicationUserDto
    {
        public EmployeeDto Employees { get; set; }
        public ManagerDto Manager { get; set; }
    }
}
