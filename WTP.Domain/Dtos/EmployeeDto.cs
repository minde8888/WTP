using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Domain.Dtos
{
    public class EmployeeDto: BaseDto
    {
        public Guid? ManagerId { get; set; }
    }
}
