using System;
using System.Collections.Generic;
using WTP.Domain.Dtos;

namespace WTP.Services.Services.Dtos
{
    public class EmployeeInformationDto : BaseEntityDto
    {
        public ICollection<EmployeeDto> Employees { get; set; }
        public string Token { get; set; }
    }
}