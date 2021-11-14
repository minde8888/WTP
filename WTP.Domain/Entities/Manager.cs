using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.Domain.Entities
{
    public class Manager : BaseEntity
    {
        public ICollection<Employee> Employees { get; set; }
    }
}
