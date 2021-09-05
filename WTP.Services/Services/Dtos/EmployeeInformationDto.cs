using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Domain.Dtos;

namespace WTP.Services.Services.Dtos
{
    public class EmployeeInformationDto : BaseEntityDto
    {
        public ICollection<EmployeeDto> Employees { get; set; }

        public Guid? ManagerId { get; set; }

        public string Role { get; set; }
    }
}
