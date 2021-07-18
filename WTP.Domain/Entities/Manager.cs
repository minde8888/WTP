using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.Domain.Entities
{
    public class Manager : BaseEntyti
    {
        public List<Employee> Employees { get; set; }
    }
}
