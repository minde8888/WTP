using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.Domain.Entities
{
    public class Manager : BaseEntiy
    {
        public ICollection<Employee> Employees { get; set; }
    }
}
