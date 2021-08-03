using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace WTP.Domain.Dtos
{
    public class ManagerDto : BaseEntityDto
    {
        public ICollection<EmployeeDto> Employees { get; set; }
    }
}
