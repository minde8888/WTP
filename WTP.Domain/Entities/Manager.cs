using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WTP.Domain.Entities
{
    public class Manager : BaseEntity
    {
        public ICollection<Employee> Employees { get; set; }
        public Guid? ProjectId { get; set; }
        public ICollection<Project> Project { get; set; }
    }
}
