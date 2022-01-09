using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Domain.Dtos
{
    public class ProjectDto
    {
        public Guid ProjectId { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public string Status { get; set; }
        public ICollection<EmployeeDto> Employees { get; set; }
        public ICollection<ManagerDto> Manager { get; set; }
    }
}
