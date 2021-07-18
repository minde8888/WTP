using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Domain.Dtos
{
   public class PostDto
    {
        public string Title { get; set; }
        public string Context { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
    }
}
