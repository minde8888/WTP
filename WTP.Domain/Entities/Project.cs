using System;
using System.Collections.Generic;

namespace WTP.Domain.Entities
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Manager> Manager { get; set; }
    }
}
