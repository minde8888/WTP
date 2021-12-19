using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.Domain.Entities
{
    public class Address 
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public Guid? ManagerId { get; set; }
        public Manager Manager { get; set; }
        public Guid? EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
